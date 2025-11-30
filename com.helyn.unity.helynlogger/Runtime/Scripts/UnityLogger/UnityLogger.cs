// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Microsoft.Extensions.Logging;
using System;
using UnityEngine;

namespace Helyn.Logger
{
	public class UnityLogger : Microsoft.Extensions.Logging.ILogger
	{
		private readonly string categoryName;

		public UnityLogger(string categoryName)
		{
			this.categoryName = categoryName;
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return null;
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return LoggerFilter.IsEnabled(categoryName, logLevel);
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel)
			   || formatter == null)
			{
				return;
			}

			string message = formatter(state, exception);
			string fullMessage = UnityLogFormatter.FormatLogMessage(logLevel, categoryName, message);
			switch (logLevel)
			{
				case LogLevel.Critical:
				case LogLevel.Error:
					Debug.LogError(fullMessage);
					break;

				case LogLevel.Warning:
					Debug.LogWarning(fullMessage);
					break;

				default:
					Debug.Log(fullMessage);
					break;
			}

			if (exception != null)
			{
				Debug.LogException(exception);
			}
		}
	}
}