namespace Logitech.Api.Data;

/// <summary>
/// Warranty details for a device.
/// </summary>
public class DeviceWarranty
{
	/// <summary>Warranty or service type.</summary>
	public string Type { get; set; } = string.Empty;

	/// <summary>Expiration timestamp (epoch milliseconds).</summary>
	public long ExpiresAt { get; set; }
}
