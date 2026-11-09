using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Logitech.Api.Test;

/// <summary>
/// Offline tests of the mTLS handler and client construction, using a throwaway self-signed certificate.
/// </summary>
public sealed class HandlerTests
{
	private static LogitechSyncClientOptions Options(ILogger? logger = null)
	{
		using var rsa = RSA.Create(2048);
		var request = new CertificateRequest("CN=Logitech.Api.Test", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
		using var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddMinutes(-1), DateTimeOffset.UtcNow.AddHours(1));
		return new LogitechSyncClientOptions
		{
			Certificate = certificate.ExportCertificatePem(),
			PrivateKey = rsa.ExportPkcs8PrivateKeyPem(),
			Logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance
		};
	}

	[Fact]
	public void ClientCtor_WithAValidCertificate_ExposesPlaces()
		=> new LogitechSyncClient(Options()).Places.Should().NotBeNull();

	[Theory]
	[InlineData("POST")]
	[InlineData("PUT")]
	[InlineData("PATCH")]
	[InlineData("DELETE")]
	public async Task WriteWhenNotPermitted_IsRefusedBeforeSending(string method)
	{
		var logger = new ListLogger();
		using var handler = new AuthenticatedHttpHandler(Options(logger));
		using var client = new HttpClient(handler);
		using var request = new HttpRequestMessage(new HttpMethod(method), "https://logitech.invalid/v1/x")
		{
			Content = new StringContent("{\"name\":\"desk\"}")
		};

		var act = () => client.SendAsync(request, TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Write operations are not permitted*");
		logger.Messages.Should().Contain(m => m.Contains("{\"name\":\"desk\"}", StringComparison.Ordinal));
	}

	private sealed class ListLogger : ILogger
	{
		public List<string> Messages { get; } = [];

		public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

		public bool IsEnabled(LogLevel logLevel) => true;

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
			=> Messages.Add(formatter(state, exception));
	}
}
