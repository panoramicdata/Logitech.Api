namespace Logitech.Api;

/// <summary>
/// Configuration options for <see cref="LogitechSyncClient"/>.
/// </summary>
public class LogitechSyncClientOptions
{
	/// <summary>
	/// Optional logger for client-side diagnostics.
	/// </summary>
	public ILogger Logger { get; init; } = NullLogger.Instance;

	/// <summary>
	/// Optional client certificate used for mTLS authentication.
	/// </summary>
	public required string Certificate { get; init; }

	/// <summary>
	/// Controls whether write HTTP methods are allowed.
	/// </summary>
	public bool IsWritePermitted { get; set; }

	/// <summary>
	///	Mandatory private key in PEM format used for signing requests. This should be the private key corresponding to the public key in the client certificate.
	/// </summary>
	public required string PrivateKey { get; init; }
}
