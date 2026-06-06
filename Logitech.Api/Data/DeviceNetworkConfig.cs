namespace Logitech.Api.Data;

/// <summary>
/// Represents network settings for an interface.
/// </summary>
public class DeviceNetworkConfig
{
	/// <summary>Addressing mode (for example dhcp/manual).</summary>
	public string Mode { get; set; } = string.Empty;

	/// <summary>Assigned IP address.</summary>
	public string? Address { get; set; }

	/// <summary>Gateway address.</summary>
	public string? Gateway { get; set; }

	/// <summary>Subnet mask.</summary>
	public string? SubnetMask { get; set; }

	/// <summary>DNS server list.</summary>
	public List<string>? Dns { get; set; }
}
