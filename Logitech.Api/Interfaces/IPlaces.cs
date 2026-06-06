namespace Logitech.Api.Interfaces;

/// <summary>
/// Refit contract for place-related Logitech Sync API operations.
/// </summary>
public interface IPlaces
{
	/// <summary>
	/// Gets rooms and desks for an organization, including optional device data.
	/// </summary>
	/// <param name="orgId">The organization ID.</param>
	/// <param name="request">The request containing query parameters.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A page of places and an optional continuation token.</returns>
	[Get("/org/{orgId}/place")]
	Task<PlaceResponse> GetAsync(
		string orgId,
		[Query] PlaceRequest request,
		CancellationToken cancellationToken);
}
