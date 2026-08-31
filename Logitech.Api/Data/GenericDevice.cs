namespace Logitech.Api.Data;

/// <summary>
/// Represents a non-Logitech peripheral.
/// </summary>
public class GenericDevice : DeviceBase
{
	/// <summary>Firmware or software version.</summary>
	public string? Version { get; set; }
}
