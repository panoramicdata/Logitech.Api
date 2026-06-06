using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;

namespace Logitech.Api;

/// <summary>
/// Configuration options for <see cref="LogitechSyncClient"/>.
/// </summary>
public class LogitechSyncClientOptions
{
	/// <summary>
	/// Optional logger for client-side diagnostics.
	/// </summary>
	public ILogger? Logger { get; set; }

	/// <summary>
	/// Optional client certificate used for mTLS authentication.
	/// </summary>
	public X509Certificate2? ClientCertificate { get; set; }

	/// <summary>
	/// Controls whether write HTTP methods are allowed.
	/// </summary>
	public bool IsWritePermitted { get; set; } = false;
}
