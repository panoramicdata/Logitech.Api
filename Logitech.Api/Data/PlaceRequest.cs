namespace Logitech.Api.Data;

/// <summary>
/// Request parameters for fetching places in a Logitech Sync organization.
/// </summary>
public class PlaceRequest
{
	/// <summary>
	/// Pagination token returned by a previous call.
	/// </summary>
	public string? Continuation { get; set; }

	/// <summary>
	/// Maximum number of results to return, between 1 and 1000.
	/// </summary>
	public int? Limit { get; set; }

	/// <summary>
	/// When true, include room places.
	/// </summary>
	public bool? Rooms { get; set; }

	/// <summary>
	/// When true, include desk places.
	/// </summary>
	public bool? Desks { get; set; }

	/// <summary>
	/// When true, include unlicensed places with basic information.
	/// </summary>
	public bool? Unlicensed { get; set; }

	/// <summary>
	/// Comma-separated list of projection fields.
	/// </summary>
	public string? Projection { get; set; }
}