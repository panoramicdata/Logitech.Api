# Logitech.Api

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/b9ee3816316d4888ad5d9bc2203a6437)](https://app.codacy.com/gh/panoramicdata/Logitech.Api/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![NuGet Version](https://img.shields.io/nuget/v/Logitech.Api)](https://www.nuget.org/packages/Logitech.Api)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Logitech.Api)](https://www.nuget.org/packages/Logitech.Api)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Introduction

Logitech.Api is a .NET 10 client library for the Logitech Sync Cloud API.

The current OpenAPI definition in this repository documents a single endpoint:

- `GET /org/{orgId}/place`

This library is designed around that contract, with strongly typed models, mTLS support, and safety controls for write operations.

OpenAPI source used by this package: [Docs/openapi-spec](Docs/openapi-spec)

## Installation

```bash
dotnet add package Logitech.Api
```

## Requirements

- .NET 10 SDK/runtime
- A Logitech Sync organization ID
- A valid client certificate for Logitech Sync API mTLS authentication

## Quick Start

```csharp
using Logitech.Api;
using System.Security.Cryptography.X509Certificates;

var certificate = X509CertificateLoader.LoadPkcs12FromFile(
	"path/to/logitech-client-certificate.pfx",
	"certificate-password");

using var httpClient = new HttpClient
{
	BaseAddress = new Uri("https://api.sync.logitech.com/v1/")
};

var client = new LogitechSyncClient(
	httpClient,
	new LogitechSyncClientOptions
	{
		ClientCertificate = certificate,
		IsWritePermitted = false
	});

var response = await client.Places.GetAsync(
	orgId: "YOUR_ORG_ID",
	limit: 100,
	rooms: true,
	desks: true,
	unlicensed: false,
	projection: "place.info,place.occupancy,place.device,place.device.info,place.device.status");
```

## API Surface

| Property | Endpoint | Description |
|----------|----------|-------------|
| `Places` | `GET /org/{orgId}/place` | Returns rooms/desks and optionally nested device information |

### Query Parameters for `Places.GetAsync`

| Parameter | Type | Description |
|----------|------|-------------|
| `orgId` | `string` | Required organization ID |
| `continuation` | `string?` | Pagination token returned by previous call |
| `limit` | `int?` | Max results per page (1-1000) |
| `rooms` | `bool?` | Include rooms |
| `desks` | `bool?` | Include desks |
| `unlicensed` | `bool?` | Include unlicensed places with basic fields |
| `projection` | `string?` | Comma-separated field projection list |

## Safety Model

`LogitechSyncClientOptions.IsWritePermitted` defaults to `false`.

When `false`, the internal handler blocks all outgoing `POST`, `PUT`, `PATCH`, and `DELETE` requests with an `InvalidOperationException`.

This helps prevent accidental mutations when running in read-only mode.

## Error Handling and Throttling

Based on the OpenAPI definition, you should expect at least:

- `400 Bad Request` for invalid query parameters
- `403 Forbidden` for invalid/expired certificates or organization access
- `429 Too Many Requests` when quota/rate limits are exceeded

The API documentation recommends implementing retries with exponential backoff for throttled (`429`) responses.

## Testing

This repository includes both unit tests and integration tests.

Integration tests use user-secrets. Configure the following values:

```json
{
  "Logitech": {
	"OrgId": "YOUR_ORG_ID",
	"CertificatePath": "PATH_TO_YOUR_CERT.pfx",
	"CertificatePassword": "YOUR_CERT_PASSWORD"
  }
}
```

Reference files:

- [Logitech.Api.Test/secrets.example.json](Logitech.Api.Test/secrets.example.json)
- [Logitech.Api.Test/secrets.schema.json](Logitech.Api.Test/secrets.schema.json)

Run tests:

```bash
dotnet test Logitech.Api.Test/Logitech.Api.Test.csproj -v minimal
```

Collect coverage:

```bash
dotnet test Logitech.Api.Test/Logitech.Api.Test.csproj -v minimal --collect:"XPlat Code Coverage"
```

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE).

## Copyright

Copyright (c) 2026 Panoramic Data Limited. All rights reserved.

