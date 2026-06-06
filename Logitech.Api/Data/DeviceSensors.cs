namespace Logitech.Api.Data;

/// <summary>
/// Live environmental sensor readings for supported devices.
/// </summary>
public class DeviceSensors
{
	/// <summary>Latest sensor timestamp (epoch milliseconds).</summary>
	public long LatestTs { get; set; }

	/// <summary>CO2 concentration (ppm).</summary>
	public int? Co2 { get; set; }

	/// <summary>Atmospheric pressure (Pa).</summary>
	public int? Pressure { get; set; }

	/// <summary>Temperature in Celsius.</summary>
	public double? Temperature { get; set; }

	/// <summary>Relative humidity percentage.</summary>
	public double? Humidity { get; set; }

	/// <summary>Total volatile organic compounds.</summary>
	public int? Tvoc { get; set; }

	/// <summary>VOC index.</summary>
	public int? VocIndex { get; set; }

	/// <summary>Particulate matter PM10.</summary>
	public int? Pm10 { get; set; }

	/// <summary>Particulate matter PM2.5.</summary>
	public int? Pm25 { get; set; }

	/// <summary>Presence detection state.</summary>
	public int? Presence { get; set; }
}
