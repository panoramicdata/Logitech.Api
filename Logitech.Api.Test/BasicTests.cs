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

	[Fact]
	public void ClientCtor_CreatesRefitInterface()
	{
		using var cert = CreateTestCertificate();
		var client = new LogitechSyncClient(CreateOptions(cert));

		client.Places.Should().NotBeNull();
	}

	[Theory]
	[InlineData("POST")]
	[InlineData("PUT")]
	[InlineData("PATCH")]
	[InlineData("DELETE")]
	public async Task AuthenticatedHandler_BlocksWriteMethodsWhenReadOnly(string method)
	{
		using var cert = CreateTestCertificate();
		AuthenticatedHttpHandler handler = new(CreateOptions(cert, isWritePermitted: false))
		{
			InnerHandler = new StubHttpMessageHandler()
		};
		using HttpMessageInvoker invoker = new(handler);

		HttpRequestMessage request = new(new HttpMethod(method), "https://example.test/resource");

		Func<Task<HttpResponseMessage>> act = () => invoker.SendAsync(request, CancellationToken.None);

		var ex = await act.Should().ThrowAsync<InvalidOperationException>();
		ex.Which.Message.Should().Contain("Write operations are not permitted");
	}

	[Fact]
	public async Task AuthenticatedHandler_AllowsGetWhenReadOnly()
	{
		using var cert = CreateTestCertificate();
		AuthenticatedHttpHandler handler = new(CreateOptions(cert, isWritePermitted: false))
		{
			InnerHandler = new StubHttpMessageHandler()
		};
		using HttpMessageInvoker invoker = new(handler);

		var request = new HttpRequestMessage(HttpMethod.Get, "https://example.test/resource");
		var response = await invoker.SendAsync(request, CancellationToken.None);

		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task AuthenticatedHandler_AllowsWriteWhenPermitted()
	{
		using var cert = CreateTestCertificate();
		AuthenticatedHttpHandler handler = new(CreateOptions(cert, isWritePermitted: true))
		{
			InnerHandler = new StubHttpMessageHandler()
		};
		using HttpMessageInvoker invoker = new(handler);

		var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/resource");
		var response = await invoker.SendAsync(request, CancellationToken.None);

		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task AuthenticatedHandler_ThrowsWhenRequestIsNull()
	{
		using var cert = CreateTestCertificate();
		TestableAuthenticatedHttpHandler handler = new(CreateOptions(cert));

		Func<Task> act = () => handler.InvokeSendAsync(null, CancellationToken.None);

		await act.Should().ThrowAsync<ArgumentNullException>();
	}

	[Fact]
	public void AuthenticatedHandler_AcceptsClientCertificate()
	{
		using var cert = CreateTestCertificate();
		AuthenticatedHttpHandler handler = new(CreateOptions(cert));

		handler.InnerHandler.Should().BeOfType<HttpClientHandler>();
	}

	private static LogitechSyncClientOptions CreateOptions(X509Certificate2 certificate, bool isWritePermitted = false) => new()
	{
		Certificate = certificate,
		PrivateKey = "test-private-key",
		IsWritePermitted = isWritePermitted,
		Logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance
	};

	private static X509Certificate2 CreateTestCertificate()
	{
		using var rsa = RSA.Create(2048);
		CertificateRequest request = new("CN=LogitechApiTests", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
		return request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1));
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
