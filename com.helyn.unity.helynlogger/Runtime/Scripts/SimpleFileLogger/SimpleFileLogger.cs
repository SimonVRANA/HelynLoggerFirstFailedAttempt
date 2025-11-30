// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Helyn.Logger
{
	public class SimpleFileLogger : ILogger
	{
		private readonly string categoryName;
		private readonly string logFilePath;

		public SimpleFileLogger(string categoryName, string logFilePath)
		{
			this.categoryName = categoryName;
			this.logFilePath = logFilePath;
		}

		public IDisposable BeginScope<TState>(TState state) where TState : notnull
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

			string rawMessage = formatter(state, exception);

			// Apply formatting (from user settings)
			string finalMessage = SimpleFileLoggerFormatter.ApplyFormat(rawMessage,
																		logLevel,
																		categoryName);

			// If there is an exception, append it manually (file logger does not auto-print exceptions)
			if (exception != null)
			{
				finalMessage += Environment.NewLine + exception;
			}

			try
			{
				File.AppendAllText(logFilePath, finalMessage + Environment.NewLine);
			}
			catch
			{
				// NEVER throw inside logging. Silent failure is standard.
			}
		}
	}
}