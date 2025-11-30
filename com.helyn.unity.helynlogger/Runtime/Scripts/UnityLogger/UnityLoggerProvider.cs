// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Helyn.Logger
{
	public class UnityLoggerProvider : ILoggerProvider
	{
		private readonly ConcurrentDictionary<string, Microsoft.Extensions.Logging.ILogger> loggers = new();

		public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
		{
			return loggers.GetOrAdd(categoryName, name => new UnityLogger(name));
		}

		public void Dispose()
		{
			loggers.Clear();
		}
	}
}