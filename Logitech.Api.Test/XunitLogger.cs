using Microsoft.Extensions.Logging;

namespace Logitech.Api.Test;

internal sealed class XunitLogger(ITestOutputHelper output, string categoryName = "Test") : ILogger
{
	private sealed class NullScope : IDisposable
	{
		public static readonly NullScope Instance = new();

		public void Dispose()
		{
		}
	}

	public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;

	public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
	{
		if (!IsEnabled(logLevel))
		{
			return;
		}

		var message = formatter(state, exception);
		output.WriteLine($"[{logLevel}] {categoryName}: {message}");

		if (exception is not null)
		{
			output.WriteLine(exception.ToString());
		}
	}
}
