using Logitech.Api.Interfaces;

namespace Logitech.Api;

/// <summary>
/// Main entry point for Logitech Sync API operations.
/// </summary>
public class LogitechSyncClient
{
	private static readonly JsonSerializerOptions _serializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
	};

	/// <summary>
	/// Initializes a new instance of the <see cref="LogitechSyncClient"/> class.
	/// </summary>
	/// <param name="options">Client options including mTLS certificate and write-permission settings.</param>
	public LogitechSyncClient(LogitechSyncClientOptions options)
	{
		ArgumentNullException.ThrowIfNull(options);

		var handler = new AuthenticatedHttpHandler(options);

		var httpClient = new HttpClient(handler)
		{
			BaseAddress = new Uri("https://api.sync.logitech.com/v1/")
		};

		var client = new HttpClient(handler)
		{
			BaseAddress = httpClient.BaseAddress,
			Timeout = httpClient.Timeout
		};

		foreach (var header in httpClient.DefaultRequestHeaders)
		{
			client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
		}

		var refitSettings = new RefitSettings
		{
			ContentSerializer = new SystemTextJsonContentSerializer(_serializerOptions)
		};

		Places = RestService.For<IPlaces>(client, refitSettings);
	}

	/// <summary>
	/// Place operations for rooms and desks.
	/// </summary>
	public IPlaces Places { get; }
}
