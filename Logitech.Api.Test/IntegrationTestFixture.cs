namespace Logitech.Api.Test;

public sealed class IntegrationTestFixture : IDisposable
{
	private readonly IConfiguration _configuration;

	public IntegrationTestFixture()
	{
		_configuration = new ConfigurationBuilder()
			.AddUserSecrets<IntegrationTestFixture>()
			.Build();

		var certificateString = GetRequiredValue("Logitech:Certificate");

		Certificate = LoadCertificate(certificateString);
		PrivateKey = GetRequiredValue("Logitech:PrivateKey");
		OrgId = GetRequiredValue("Logitech:OrgId");
	}

	public string OrgId { get; }

	public string PrivateKey { get; }

	public X509Certificate2 Certificate { get; }

	public LogitechSyncClient CreateClient(ITestOutputHelper output) => new(
		new LogitechSyncClientOptions
		{
			Certificate = Certificate,
			PrivateKey = PrivateKey,
			Logger = new XunitLogger(output)
		});

	public void Dispose() => Certificate.Dispose();

	private static X509Certificate2 LoadCertificate(string certificateString)
	{
		if (!certificateString.Contains("BEGIN CERTIFICATE", StringComparison.OrdinalIgnoreCase))
		{
			throw new FormatException("Certificate string must be in PEM format, including BEGIN CERTIFICATE and END CERTIFICATE lines.");
		}

		return X509Certificate2.CreateFromPem(certificateString);
	}

	private string GetRequiredValue(string key)
	{
		var value = _configuration[key];
		value.Should().NotBeNullOrWhiteSpace($"{key} must be configured in user secrets.");
		return value!;
	}
}
