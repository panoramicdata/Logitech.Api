namespace Logitech.Api.Data;

/// <summary>
/// Detailed network information for a device.
/// </summary>
public class DeviceNetwork
{
	/// <summary>IP address.</summary>
	public string? Ip { get; set; }

	/// <summary>MAC address.</summary>
	public string? Mac { get; set; }

	/// <summary>Host name.</summary>
	public string? HostName { get; set; }

	/// <summary>Wired interface configuration.</summary>
	public DeviceNetworkConfig? Wired { get; set; }

	/// <summary>Wireless interface configuration.</summary>
	public DeviceNetworkConfig? Wireless { get; set; }
}
