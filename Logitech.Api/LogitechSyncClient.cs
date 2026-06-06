using Logitech.Api.Interfaces;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace Logitech.Api;

/// <summary>
/// Main entry point for Logitech Sync API operations.
/// </summary>
public class LogitechSyncClient
{
	private static readonly JsonSerializerOptions SerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
	};

	/// <summary>
	/// Initializes a new instance of the <see cref="LogitechSyncClient"/> class.
	/// </summary>
	/// <param name="httpClient">The source <see cref="HttpClient"/> containing base address and headers.</param>
	/// <param name="options">Client options including mTLS certificate and write-permission settings.</param>
	public LogitechSyncClient(HttpClient httpClient, LogitechSyncClientOptions options)
	{
		ArgumentNullException.ThrowIfNull(httpClient);
		ArgumentNullException.ThrowIfNull(options);

		if (httpClient.BaseAddress is null)
		{
			throw new ArgumentException("The provided HttpClient must have a BaseAddress.", nameof(httpClient));
		}

		var handler = new AuthenticatedHttpHandler(options)
		{
			InnerHandler = new HttpClientHandler()
		};

		var client = new HttpClient(handler)
		{
			BaseAddress = httpClient.BaseAddress,
			Timeout = httpClient.Timeout
		};

		foreach (KeyValuePair<string, IEnumerable<string>> header in httpClient.DefaultRequestHeaders)
		{
			client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
		}

		var refitSettings = new RefitSettings
		{
			ContentSerializer = new SystemTextJsonContentSerializer(SerializerOptions)
		};

		Places = RestService.For<IPlaces>(client, refitSettings);
	}

	/// <summary>
	/// Place operations for rooms and desks.
	/// </summary>
	public IPlaces Places { get; }
}
