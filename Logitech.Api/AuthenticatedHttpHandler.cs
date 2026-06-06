using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Logitech.Api;

/// <summary>
/// Delegating handler that enforces write-protection and applies mTLS certificate configuration.
/// </summary>
public class AuthenticatedHttpHandler : DelegatingHandler
{
	private readonly LogitechSyncClientOptions _options;

	/// <summary>
	/// Initializes a new instance of the <see cref="AuthenticatedHttpHandler"/> class.
	/// </summary>
	/// <param name="options">Client options controlling certificate usage and write permissions.</param>
	public AuthenticatedHttpHandler(LogitechSyncClientOptions options)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));

		var innerHandler = new HttpClientHandler();
		if (_options.ClientCertificate is not null)
		{
			innerHandler.ClientCertificates.Add(_options.ClientCertificate);
		}

		InnerHandler = innerHandler;
	}

	/// <summary>
	/// Sends an HTTP request after validating write-permission policy.
	/// </summary>
	/// <param name="request">The request message.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The HTTP response.</returns>
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (request is null)
		{
			throw new ArgumentNullException(nameof(request));
		}

		if (!_options.IsWritePermitted)
		{
			if (IsWriteMethod(request.Method))
			{
				throw new InvalidOperationException("Write operations are not permitted. Set IsWritePermitted to true in LogitechSyncClientOptions to allow this operation.");
			}
		}

		return base.SendAsync(request, cancellationToken);
	}

	private static bool IsWriteMethod(HttpMethod method)
	{
		return method == HttpMethod.Post ||
			method == HttpMethod.Put ||
			method == HttpMethod.Patch ||
			method == HttpMethod.Delete;
	}
}
