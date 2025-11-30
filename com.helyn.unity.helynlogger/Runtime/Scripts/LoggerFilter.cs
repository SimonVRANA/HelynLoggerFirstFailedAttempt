// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Helyn.Logger
{
	public static class LoggerFilter
	{
		private static ILoggerFactory filterFactory;
		private static readonly ConcurrentDictionary<string, ILogger> filterLoggers = new();

		public static bool IsEnabled(string categoryName, LogLevel logLevel)
		{
			EnsureFilterFactory();

			ILogger logger = filterLoggers.GetOrAdd(categoryName, filterFactory.CreateLogger(categoryName));
			return logger.IsEnabled(logLevel);
		}

		private static void EnsureFilterFactory()
		{
			if (filterFactory != null)
			{
				return;
			}

			lock (typeof(LoggerFilter))
			{
				if (filterFactory != null)
				{
					return;
				}

				// Create a factory with only configuration; no providers
				filterFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
				{
					builder.AddConfiguration(LoggerSetup.Configuration.GetSection("Logging"));
				});
			}
		}

		public static void Dispose()
		{
			lock (typeof(LoggerFilter))
			{
				filterFactory?.Dispose();
				filterFactory = null;
				filterLoggers.Clear();
			}
		}
	}
}