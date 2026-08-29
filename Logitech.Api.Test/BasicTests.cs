namespace Logitech.Api.Test;

public sealed class BasicTests
{
	[Fact]
	public void HandlerCtor_ThrowsWhenOptionsAreNull()
	{
		Action act = () => _ = new AuthenticatedHttpHandler(null!);

		act.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void ClientCtor_ThrowsWhenOptionsAreNull()
	{
		Action act = () => _ = new LogitechSyncClient(null!);

		act.Should().Throw<ArgumentNullException>();
	}
}
