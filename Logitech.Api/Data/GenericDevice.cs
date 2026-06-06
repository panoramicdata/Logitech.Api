namespace Logitech.Api.Data;

/// <summary>
/// Represents a non-Logitech peripheral.
/// </summary>
public class GenericDevice
{
	/// <summary>Unique identifier.</summary>
	public string Id { get; set; } = string.Empty;
	/// <summary>Device type.</summary>
	public string Type { get; set; } = string.Empty;
	/// <summary>Device name.</summary>
	public string Name { get; set; } = string.Empty;
	/// <summary>Firmware or software version.</summary>
	public string? Version { get; set; }
	/// <summary>Operational status.</summary>
	public string Status { get; set; } = string.Empty;
	/// <summary>Health status.</summary>
	public string HealthStatus { get; set; } = string.Empty;
	/// <summary>Last seen timestamp (epoch milliseconds).</summary>
	public long LastSeen { get; set; }
	/// <summary>Creation timestamp in Sync (epoch milliseconds).</summary>
	public long CreatedAt { get; set; }
}
