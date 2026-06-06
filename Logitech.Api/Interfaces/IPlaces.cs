using Logitech.Api.Data;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Logitech.Api.Interfaces;

/// <summary>
/// Refit contract for place-related Logitech Sync API operations.
/// </summary>
public interface IPlaces
{
	/// <summary>
	/// Gets rooms and desks for an organization, including optional device data.
	/// </summary>
	/// <param name="orgId">The Logitech Sync organization identifier.</param>
	/// <param name="continuation">Pagination token returned by a previous call.</param>
	/// <param name="limit">Maximum number of results to return, between 1 and 1000.</param>
	/// <param name="rooms">When true, include room places.</param>
	/// <param name="desks">When true, include desk places.</param>
	/// <param name="unlicensed">When true, include unlicensed places with basic information.</param>
	/// <param name="projection">Comma-separated list of projection fields.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A page of places and an optional continuation token.</returns>
	[Get("/org/{orgId}/place")]
	Task<PlaceResponse> GetAsync(
		string orgId,
		string? continuation = null,
		int? limit = null,
		bool? rooms = null,
		bool? desks = null,
		bool? unlicensed = null,
		string? projection = null,
		CancellationToken cancellationToken = default);
}
