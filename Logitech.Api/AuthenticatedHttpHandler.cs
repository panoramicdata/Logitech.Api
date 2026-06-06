using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Logitech.Api;

/// <summary>
/// Delegating handler that enforces write-protection and applies mTLS certificate configuration.
/// </summary>
internal class AuthenticatedHttpHandler : DelegatingHandler
{
	private readonly LogitechSyncClientOptions _options;
	private readonly ILogger _logger;
	private readonly X509Certificate2 _clientCertificate;

	/// <summary>
	/// Initializes a new instance of the <see cref="AuthenticatedHttpHandler"/> class.
	/// </summary>
	/// <param name="options">Client options controlling certificate usage and write permissions.</param>
	internal AuthenticatedHttpHandler(LogitechSyncClientOptions options)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));

		using var tlsCert = X509Certificate2.CreateFromPem(_options.Certificate, _options.PrivateKey);

		var rawData = tlsCert.Export(X509ContentType.Pfx);
		_clientCertificate = X509CertificateLoader.LoadPkcs12(
			rawData,
			password: null,
			X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

		var innerHandler = new HttpClientHandler
		{
			ClientCertificateOptions = ClientCertificateOption.Manual
		};
		innerHandler.ClientCertificates.Add(_clientCertificate);

		InnerHandler = innerHandler;

		_logger = _options.Logger;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_clientCertificate.Dispose();
		}

		base.Dispose(disposing);
	}

	/// <summary>
	/// Sends an HTTP request after validating write-permission policy.
	/// </summary>
	/// <param name="request">The request message.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The HTTP response.</returns>
	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		var requestId = Guid.NewGuid();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("Request {RequestId}: Sending HTTP request: {Method} {Uri}", requestId, request.Method, request.RequestUri);

			if (request.Content != null)
			{
				var content = await request.Content.ReadAsStringAsync(cancellationToken);
				_logger.LogDebug("Request {RequestId}: Content {Content}", requestId, content);
			}
		}

		if (!_options.IsWritePermitted)
		{
			if (IsWriteMethod(request.Method))
			{
				throw new InvalidOperationException("Write operations are not permitted. Set IsWritePermitted to true in LogitechSyncClientOptions to allow this operation.");
			}
		}

		var response = await base.SendAsync(request, cancellationToken);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("Request {RequestId}: Received HTTP response: {StatusCode}", requestId, response.StatusCode);

			if (response.Content != null)
			{
				var content = await response.Content.ReadAsStringAsync(cancellationToken);
				_logger.LogDebug("Request {RequestId}: Response Content {Content}", requestId, content);
			}
		}

		return response;
	}

	private static bool IsWriteMethod(HttpMethod method) => method == HttpMethod.Post ||
			method == HttpMethod.Put ||
			method == HttpMethod.Patch ||
			method == HttpMethod.Delete;
}
