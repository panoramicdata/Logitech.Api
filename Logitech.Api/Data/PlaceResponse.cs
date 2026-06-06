namespace Logitech.Api.Data;

/// <summary>
/// Response payload for organization place queries.
/// </summary>
public class PlaceResponse
{
	/// <summary>Collection of place payloads as raw JSON elements.</summary>
	public List<JsonElement> Places { get; set; } = new();

	/// <summary>Pagination continuation token, when additional pages exist.</summary>
	public string? Continuation { get; set; }
}
