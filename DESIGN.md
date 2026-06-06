# Design

This document outlines the design of the `Logitech.Api` .NET client library.

## Architecture

The library is built using a modern .NET stack, centered around the following key principles:

-   **Refit**: The library uses [Refit](https://github.com/reactiveui/refit) to create a type-safe, declarative REST client. API endpoints are defined as C# interfaces, and Refit generates the implementation at runtime. This approach simplifies API calls and makes the code easier to read and maintain.
-   **`HttpClient` Integration**: The core of the communication is handled by `HttpClient`. This allows for flexible configuration, including the use of custom message handlers for authentication, logging, and other cross-cutting concerns.
-   **System.Text.Json**: For serialization and deserialization, the library uses the high-performance `System.Text.Json` library. The Refit `ContentSerializer` is configured to use camelCase property naming to match the API's JSON response format.
-   **Dependency Injection**: The `LogitechSyncClient` is designed to be easily integrated into applications using dependency injection. It accepts an `HttpClient` and a `LogitechSyncClientOptions` object in its constructor.

## Core Components

### `LogitechSyncClient`

This is the main entry point for interacting with the Logitech Sync API. It exposes properties for each of the Refit interfaces, providing access to the various API endpoints.

### `LogitechSyncClientOptions`

This class provides configuration options for the `LogitechSyncClient`. It includes:

-   `Logger`: An optional `ILogger` for logging API requests and responses.
-   `IsWritePermitted`: A boolean flag that, when set to `false` (the default), prevents the client from making any write operations (POST, PUT, PATCH, DELETE).

### `AuthenticatedHttpHandler`

This `DelegatingHandler` is responsible for enforcing the `IsWritePermitted` flag. It intercepts every outgoing HTTP request and checks the following:

1.  Is `IsWritePermitted` set to `false`?
2.  Is the HTTP method a write operation (`POST`, `PUT`, `PATCH`, `DELETE`)?

If both conditions are true, the handler will throw an `InvalidOperationException` to prevent the request from being sent to the server. This provides a safety mechanism to ensure that the client cannot accidentally modify data when it is configured for read-only access.

This handler is also responsible for adding the required mTLS client certificate to the request.

## Project Structure

The solution is divided into two projects:

-   `Logitech.Api`: The main class library containing the client, interfaces, and data models. This is the project that will be packed into a NuGet package.
-   `Logitech.Api.Test`: A test project containing unit and integration tests for the library. It uses xUnit and AwesomeAssertions for testing.
