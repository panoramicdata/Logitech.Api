namespace Logitech.Api.Test;

public abstract class TestBase(IntegrationTestFixture fixture, ITestOutputHelper output)
{
	protected string OrganizationId { get; } = fixture.OrganizationId;

	protected LogitechSyncClient LogitechSyncClient { get; } = fixture.CreateClient(output);
}
