namespace Logitech.Api.Data;

/// <summary>
/// The identity, status and lifecycle properties shared by every device Sync reports,
/// whatever kind of device it is.
/// </summary>
public abstract class DeviceBase
{
	/// <summary>Unique identifier.</summary>
	public string Id { get; set; } = string.Empty;

	/// <summary>Device type.</summary>
	public string Type { get; set; } = string.Empty;

	/// <summary>Device name.</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>Operational status.</summary>
	public string Status { get; set; } = string.Empty;

	/// <summary>Health status.</summary>
	public string HealthStatus { get; set; } = string.Empty;

	/// <summary>Last seen timestamp (epoch milliseconds).</summary>
	public long LastSeen { get; set; }

	/// <summary>Creation timestamp in Sync (epoch milliseconds).</summary>
	public long CreatedAt { get; set; }
}
