namespace Logitech.Api.Data;

/// <summary>
/// Contract details for a room or desk.
/// </summary>
public class PlaceContract
{
	/// <summary>Service contract type.</summary>
	public string Service { get; set; } = string.Empty;
	/// <summary>Display name of the contract.</summary>
	public string Name { get; set; } = string.Empty;
	/// <summary>Contract number.</summary>
	public int Number { get; set; }
	/// <summary>Expiration timestamp (epoch milliseconds).</summary>
	public long ExpiresAt { get; set; }
}
