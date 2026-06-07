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

	private sealed class StubHttpMessageHandler : HttpMessageHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken _)
			=> Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
			{
				RequestMessage = request
			});
	}
}
