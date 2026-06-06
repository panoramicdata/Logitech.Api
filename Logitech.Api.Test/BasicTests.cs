using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Logitech.Api.Test;

public sealed class BasicTests
{
	[Fact]
	public void HandlerCtor_ThrowsWhenOptionsAreNull()
	{
		Assert.Throws<ArgumentNullException>(() => _ = new AuthenticatedHttpHandler(null!));
	}

	[Fact]
	public void ClientCtor_ThrowsWhenBaseAddressIsMissing()
	{
		HttpClient httpClient = new();
		LogitechSyncClientOptions options = new();

		Action act = () => _ = new LogitechSyncClient(httpClient, options);

		Assert.Throws<ArgumentException>(act);
	}

	[Fact]
	public void ClientCtor_ThrowsWhenHttpClientIsNull()
	{
		Assert.Throws<ArgumentNullException>(() => _ = new LogitechSyncClient(null!, new LogitechSyncClientOptions()));
	}

	[Fact]
	public void ClientCtor_ThrowsWhenOptionsAreNull()
	{
		HttpClient httpClient = new()
		{
			BaseAddress = new Uri("https://api.sync.logitech.com/v1/")
		};

		Assert.Throws<ArgumentNullException>(() => _ = new LogitechSyncClient(httpClient, null!));
	}

	[Fact]
	public void ClientCtor_CreatesRefitInterface()
	{
		HttpClient httpClient = new(new StubHttpMessageHandler())
		{
			BaseAddress = new Uri("https://api.sync.logitech.com/v1/")
		};

		LogitechSyncClient client = new(httpClient, new LogitechSyncClientOptions());

		Assert.NotNull(client.Places);
	}

	[Fact]
	public void ClientCtor_CopiesDefaultHeaders()
	{
		HttpClient httpClient = new(new StubHttpMessageHandler())
		{
			BaseAddress = new Uri("https://api.sync.logitech.com/v1/")
		};
		httpClient.DefaultRequestHeaders.Add("X-Test-Header", "test-value");

		LogitechSyncClient client = new(httpClient, new LogitechSyncClientOptions());

		Assert.NotNull(client.Places);
	}

	[Theory]
	[InlineData("POST")]
	[InlineData("PUT")]
	[InlineData("PATCH")]
	[InlineData("DELETE")]
	public async Task AuthenticatedHandler_BlocksWriteMethodsWhenReadOnly(string method)
	{
		AuthenticatedHttpHandler handler = new(new LogitechSyncClientOptions { IsWritePermitted = false })
		{
			InnerHandler = new StubHttpMessageHandler()
		};
		using HttpMessageInvoker invoker = new(handler);

		HttpRequestMessage request = new(new HttpMethod(method), "https://example.test/resource");

		Task<HttpResponseMessage> act() => invoker.SendAsync(request, CancellationToken.None);

		InvalidOperationException ex = await Assert.ThrowsAsync<InvalidOperationException>(act);
		Assert.Contains("Write operations are not permitted", ex.Message);
	}

	[Fact]
	public async Task AuthenticatedHandler_AllowsGetWhenReadOnly()
	{
		AuthenticatedHttpHandler handler = new(new LogitechSyncClientOptions { IsWritePermitted = false })
		{
			InnerHandler = new StubHttpMessageHandler()
		};
		using HttpMessageInvoker invoker = new(handler);

		HttpRequestMessage request = new(HttpMethod.Get, "https://example.test/resource");
		HttpResponseMessage response = await invoker.SendAsync(request, CancellationToken.None);

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}

	[Fact]
	public async Task AuthenticatedHandler_AllowsWriteWhenPermitted()
	{
		AuthenticatedHttpHandler handler = new(new LogitechSyncClientOptions { IsWritePermitted = true })
		{
			InnerHandler = new StubHttpMessageHandler()
		};
		using HttpMessageInvoker invoker = new(handler);

		HttpRequestMessage request = new(HttpMethod.Post, "https://example.test/resource");
		HttpResponseMessage response = await invoker.SendAsync(request, CancellationToken.None);

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}

	[Fact]
	public async Task AuthenticatedHandler_ThrowsWhenRequestIsNull()
	{
		TestableAuthenticatedHttpHandler handler = new(new LogitechSyncClientOptions());

		await Assert.ThrowsAsync<ArgumentNullException>(() => handler.InvokeSendAsync(null, CancellationToken.None));
	}

	[Fact]
	public void AuthenticatedHandler_AcceptsClientCertificate()
	{
		using X509Certificate2 cert = CreateTestCertificate();
		AuthenticatedHttpHandler handler = new(new LogitechSyncClientOptions
		{
			ClientCertificate = cert
		});

		Assert.IsType<HttpClientHandler>(handler.InnerHandler);
	}

	private static X509Certificate2 CreateTestCertificate()
	{
		using RSA rsa = RSA.Create(2048);
		CertificateRequest request = new("CN=LogitechApiTests", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
		return request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1));
	}

	private sealed class StubHttpMessageHandler : HttpMessageHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
			{
				RequestMessage = request
			});
		}
	}

	private sealed class TestableAuthenticatedHttpHandler : AuthenticatedHttpHandler
	{
		public TestableAuthenticatedHttpHandler(LogitechSyncClientOptions options)
			: base(options)
		{
			InnerHandler = new StubHttpMessageHandler();
		}

		public Task<HttpResponseMessage> InvokeSendAsync(HttpRequestMessage? request, CancellationToken cancellationToken)
		{
			return base.SendAsync(request!, cancellationToken);
		}
	}
}
