namespace Logitech.Api.Data;

/// <summary>
/// Basic network information for a computer device.
/// </summary>
public class ComputerDeviceNetwork
{
	/// <summary>IP address.</summary>
	public string Ip { get; set; } = string.Empty;

	/// <summary>MAC address.</summary>
	public string Mac { get; set; } = string.Empty;
}
