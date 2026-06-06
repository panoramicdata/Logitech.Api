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
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			=> Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
			{
				RequestMessage = request
			});
	}

	private sealed class TestableAuthenticatedHttpHandler : AuthenticatedHttpHandler
	{
		public TestableAuthenticatedHttpHandler(LogitechSyncClientOptions options)
			: base(options)
		{
			InnerHandler = new StubHttpMessageHandler();
		}

		public Task<HttpResponseMessage> InvokeSendAsync(HttpRequestMessage? request, CancellationToken cancellationToken) => SendAsync(request!, cancellationToken);
	}
}
