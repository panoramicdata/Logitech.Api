namespace Logitech.Api.Data;

/// <summary>
/// Represents a computer device (for example, a room PC running Sync App).
/// </summary>
public class ComputerDevice
{
	/// <summary>Unique identifier.</summary>
	public string Id { get; set; } = string.Empty;

	/// <summary>Device type.</summary>
	public string Type { get; set; } = string.Empty;

	/// <summary>Device name.</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>Software version.</summary>
	public string Version { get; set; } = string.Empty;

	/// <summary>Operational status.</summary>
	public string Status { get; set; } = string.Empty;

	/// <summary>Health status.</summary>
	public string HealthStatus { get; set; } = string.Empty;

	/// <summary>Network details, when available.</summary>
	public ComputerDeviceNetwork? Network { get; set; }

	/// <summary>Last seen timestamp (epoch milliseconds).</summary>
	public long LastSeen { get; set; }

	/// <summary>Creation timestamp in Sync (epoch milliseconds).</summary>
	public long CreatedAt { get; set; }
}
