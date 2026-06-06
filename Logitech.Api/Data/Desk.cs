using System.Collections.Generic;
using System.Text.Json;

namespace Logitech.Api.Data;

/// <summary>
/// Represents a desk place in Logitech Sync.
/// </summary>
public class Desk
{
	/// <summary>Unique identifier.</summary>
	public string Id { get; set; } = string.Empty;
	/// <summary>Place type.</summary>
	public string Type { get; set; } = string.Empty;
	/// <summary>Desk name.</summary>
	public string Name { get; set; } = string.Empty;
	/// <summary>Desk location.</summary>
	public string Location { get; set; } = string.Empty;
	/// <summary>Contract details, when present.</summary>
	public PlaceContract? Contract { get; set; }
	/// <summary>License identifier, when present.</summary>
	public string? License { get; set; }
	/// <summary>Associated raw device payloads.</summary>
	public List<JsonElement> Devices { get; set; } = new();
	/// <summary>Creation timestamp in Sync (epoch milliseconds).</summary>
	public long CreatedAt { get; set; }
}
