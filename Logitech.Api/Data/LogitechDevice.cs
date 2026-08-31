namespace Logitech.Api.Data;

/// <summary>
/// Represents a Logitech-manufactured device.
/// </summary>
public class LogitechDevice : DeviceBase
{
	/// <summary>Firmware or CollabOS version.</summary>
	public string Version { get; set; } = string.Empty;

	/// <summary>Device serial number, when available.</summary>
	public string? Serial { get; set; }

	/// <summary>Peripheral counts and details.</summary>
	public Peripherals? Peripherals { get; set; }

	/// <summary>Network details.</summary>
	public DeviceNetwork? Network { get; set; }

	/// <summary>Live sensor details.</summary>
	public DeviceSensors? Sensors { get; set; }

	/// <summary>Warranty information.</summary>
	public DeviceWarranty? Warranty { get; set; }
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
