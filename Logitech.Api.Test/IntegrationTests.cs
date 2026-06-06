using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Logitech.Api.Test;

public class IntegrationTests
{
	private readonly ITestOutputHelper _output;
	private readonly IConfiguration _configuration;

	public IntegrationTests(ITestOutputHelper output)
	{
		_output = output;
		_configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: true)
			.AddUserSecrets<IntegrationTests>()
			.Build();
	}

	[Fact]
	public async Task GetPlaces_Succeeds()
	{
		var orgId = _configuration["Logitech:OrgId"];
		var certPath = _configuration["Logitech:CertificatePath"];
		var certPassword = _configuration["Logitech:CertificatePassword"];

		if (string.IsNullOrWhiteSpace(orgId) || string.IsNullOrWhiteSpace(certPath))
		{
			_output.WriteLine("Skipping integration test: Logitech credentials not configured in user secrets.");
			return;
		}

		var handler = new HttpClientHandler();
		var certificate = X509CertificateLoader.LoadPkcs12FromFile(certPath, certPassword);
		handler.ClientCertificates.Add(certificate);

		var httpClient = new HttpClient(handler)
		{
			BaseAddress = new System.Uri("https://api.sync.logitech.com/v1/")
		};

		var client = new LogitechSyncClient(httpClient, new LogitechSyncClientOptions());

		var response = await client.Places.GetAsync(orgId);

		Assert.NotNull(response);
		Assert.NotEmpty(response.Places);
	}
}
