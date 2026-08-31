namespace Logitech.Api.Data;

/// <summary>
/// Represents a computer device (for example, a room PC running Sync App).
/// </summary>
public class ComputerDevice : DeviceBase
{
	/// <summary>Software version.</summary>
	public string Version { get; set; } = string.Empty;

	/// <summary>Network details, when available.</summary>
	public ComputerDeviceNetwork? Network { get; set; }
}
