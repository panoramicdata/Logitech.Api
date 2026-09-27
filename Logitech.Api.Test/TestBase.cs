namespace Logitech.Api.Test;

// Every test deriving from this base calls the live Logitech Sync API with credentials from user
// secrets. CI has none, so the coverage job excludes them with --filter "Category!=Integration".
[Trait("Category", "Integration")]
public abstract class TestBase(IntegrationTestFixture fixture, ITestOutputHelper output)
{
	protected string OrganizationId { get; } = fixture.OrganizationId;

	protected LogitechSyncClient LogitechSyncClient { get; } = fixture.CreateClient(output);
}
