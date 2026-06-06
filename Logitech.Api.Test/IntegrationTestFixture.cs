namespace Logitech.Api.Test;

public sealed class IntegrationTestFixture
{
	private readonly IConfiguration _configuration;

	public IntegrationTestFixture()
	{
		_configuration = new ConfigurationBuilder()
			.AddUserSecrets<IntegrationTestFixture>()
			.Build();

		var certificateString = GetRequiredValue("Logitech:Certificate");

		Certificate = certificateString;
		PrivateKey = GetRequiredValue("Logitech:PrivateKey");
		OrganizationId = GetRequiredValue("Logitech:OrganizationId");
	}

	public string Certificate { get; }

	public string OrganizationId { get; }

	public string PrivateKey { get; }

	public LogitechSyncClient CreateClient(ITestOutputHelper output) => new(
		new LogitechSyncClientOptions
		{
			Certificate = GetRequiredValue("Logitech:Certificate"),
			PrivateKey = PrivateKey,
			Logger = new XunitLogger(output)
		});

	private string GetRequiredValue(string key)
	{
		var value = _configuration[key];
		value.Should().NotBeNullOrWhiteSpace($"{key} must be configured in user secrets.");
		return value!;
	}
}
