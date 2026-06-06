namespace Logitech.Api.Test;

public sealed class IntegrationTests(IntegrationTestFixture fixture, ITestOutputHelper output)
	: TestBase(fixture, output), IClassFixture<IntegrationTestFixture>
{
	[Fact]
	public async Task GetPlaces_Succeeds()
	{
		var response = await LogitechSyncClient.Places.GetAsync(
			OrganizationId,
			new PlaceRequest
			{
			},
			TestContext.Current.CancellationToken);

		response.Should().NotBeNull();
		response.Places.Should().NotBeNullOrEmpty();
	}
}
