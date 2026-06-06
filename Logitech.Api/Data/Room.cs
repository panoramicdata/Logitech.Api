namespace Logitech.Api.Data;

/// <summary>
/// Represents a room place in Logitech Sync.
/// </summary>
public class Room
{
	/// <summary>Unique identifier.</summary>
	public string Id { get; set; } = string.Empty;
	/// <summary>Place type.</summary>
	public string Type { get; set; } = string.Empty;
	/// <summary>Room name.</summary>
	public string Name { get; set; } = string.Empty;
	/// <summary>Room group path.</summary>
	public string Group { get; set; } = string.Empty;
	/// <summary>Room location path.</summary>
	public string Location { get; set; } = string.Empty;
	/// <summary>Configured seat count.</summary>
	public int SeatCount { get; set; }
	/// <summary>Current occupancy value.</summary>
	public int Occupancy { get; set; }
	/// <summary>Contract details, when present.</summary>
	public PlaceContract? Contract { get; set; }
	/// <summary>License identifier, when present.</summary>
	public string? License { get; set; }
	/// <summary>Associated raw device payloads.</summary>
	public List<JsonElement> Devices { get; set; } = new();
	/// <summary>Creation timestamp in Sync (epoch milliseconds).</summary>
	public long CreatedAt { get; set; }
}
