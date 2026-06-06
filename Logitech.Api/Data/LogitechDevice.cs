namespace Logitech.Api.Data;

/// <summary>
/// Represents a Logitech-manufactured device.
/// </summary>
public class LogitechDevice
{
	/// <summary>Unique identifier.</summary>
	public string Id { get; set; } = string.Empty;

	/// <summary>Device type.</summary>
	public string Type { get; set; } = string.Empty;

	/// <summary>Device name.</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>Firmware or CollabOS version.</summary>
	public string Version { get; set; } = string.Empty;

	/// <summary>Device serial number, when available.</summary>
	public string? Serial { get; set; }

	/// <summary>Operational status.</summary>
	public string Status { get; set; } = string.Empty;

	/// <summary>Health status.</summary>
	public string HealthStatus { get; set; } = string.Empty;

	/// <summary>Peripheral counts and details.</summary>
	public Peripherals? Peripherals { get; set; }

	/// <summary>Network details.</summary>
	public DeviceNetwork? Network { get; set; }

	/// <summary>Live sensor details.</summary>
	public DeviceSensors? Sensors { get; set; }

	/// <summary>Warranty information.</summary>
	public DeviceWarranty? Warranty { get; set; }

	/// <summary>Last seen timestamp (epoch milliseconds).</summary>
	public long LastSeen { get; set; }

	/// <summary>Creation timestamp in Sync (epoch milliseconds).</summary>
	public long CreatedAt { get; set; }
}

/// <summary>
/// Peripheral containers for Logitech devices.
/// </summary>
public class Peripherals
{
	/// <summary>Camera peripheral details.</summary>
	public Peripheral? Camera { get; set; }
	/// <summary>Speaker peripheral details.</summary>
	public Peripheral? Speaker { get; set; }
	/// <summary>Display hub peripheral details.</summary>
	public Peripheral? DisplayHub { get; set; }
	/// <summary>Table hub peripheral details.</summary>
	public Peripheral? TableHub { get; set; }
	/// <summary>Mic pod peripheral details.</summary>
	public Peripheral? MicPod { get; set; }
	/// <summary>Mic pod hub peripheral details.</summary>
	public Peripheral? MicPodHub { get; set; }
}

/// <summary>
/// A specific peripheral entry.
/// </summary>
public class Peripheral
{
	/// <summary>Expected and actual counts for this peripheral.</summary>
	public PeripheralCount Count { get; set; } = new();
}
