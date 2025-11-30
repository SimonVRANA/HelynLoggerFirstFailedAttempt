// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;
using UnityEngine;

namespace Helyn.Logger
{
	public class SimpleFileLoggerProvider : ILoggerProvider
	{
		private readonly string logFilePath;
		private readonly ConcurrentDictionary<string, Microsoft.Extensions.Logging.ILogger> loggers = new();

		public SimpleFileLoggerProvider()
		{
			this.logFilePath = LoggerSetup.Settings.LogFilePath;

			if (!Path.IsPathRooted(logFilePath))
			{
				logFilePath = Path.Combine(Application.streamingAssetsPath, logFilePath);
			}

			// Handle timestamp placeholder in the file name
			if (logFilePath.Contains("{timestamp}"))
			{
				string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
				logFilePath = logFilePath.Replace("{timestamp}", timestamp);
			}
			else
			{
				// Delete file if exists for fresh start
				if (File.Exists(logFilePath))
				{
					File.Delete(logFilePath);
				}
			}

			// Ensure directory exists
			Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)!);
		}

		public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
		{
			return loggers.GetOrAdd(categoryName, name => new SimpleFileLogger(name, logFilePath));
		}

		public void Dispose()
		{
			loggers.Clear();
		}
	}
}