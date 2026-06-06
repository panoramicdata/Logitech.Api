namespace Logitech.Api;

/// <summary>
/// Delegating handler that enforces write-protection and applies mTLS certificate configuration.
/// </summary>
internal class AuthenticatedHttpHandler : DelegatingHandler
{
	private readonly LogitechSyncClientOptions _options;

	/// <summary>
	/// Initializes a new instance of the <see cref="AuthenticatedHttpHandler"/> class.
	/// </summary>
	/// <param name="options">Client options controlling certificate usage and write permissions.</param>
	internal AuthenticatedHttpHandler(LogitechSyncClientOptions options)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));

		var innerHandler = new HttpClientHandler();
		innerHandler.ClientCertificates.Add(_options.Certificate);

		InnerHandler = innerHandler;
	}

	/// <summary>
	/// Sends an HTTP request after validating write-permission policy.
	/// </summary>
	/// <param name="request">The request message.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The HTTP response.</returns>
	protected override Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		if (!_options.IsWritePermitted)
		{
			if (IsWriteMethod(request.Method))
			{
				throw new InvalidOperationException("Write operations are not permitted. Set IsWritePermitted to true in LogitechSyncClientOptions to allow this operation.");
			}
		}

		return base.SendAsync(request, cancellationToken);
	}

	private static bool IsWriteMethod(HttpMethod method) => method == HttpMethod.Post ||
			method == HttpMethod.Put ||
			method == HttpMethod.Patch ||
			method == HttpMethod.Delete;
}
