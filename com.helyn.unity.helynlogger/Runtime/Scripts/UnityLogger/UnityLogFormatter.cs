// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Helyn.Logger
{
	public class UnityLogFormatter
	{
		public static string FormatLogMessage(LogLevel logLevel,
											  string category,
											  string message)
		{
			if (string.IsNullOrWhiteSpace(LoggerSetup.Settings.ConsoleLogFormat))
			{
				LoggerSetup.Settings.ConsoleLogFormat = "[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] {level} {category}: {message}";
			}

			string result = LoggerSetup.Settings.ConsoleLogFormat;

			// Timestamp with optional format
			result = Regex.Replace(
				result,
				@"\{timestamp(?::(?<fmt>[^}]+))?\}",
				match =>
				{
					string fmt = match.Groups["fmt"].Value;
					if (string.IsNullOrEmpty(fmt))
					{
						fmt = "yyyy-MM-dd HH:mm:ss.fff"; // default
					}

					return DateTime.Now.ToString(fmt);
				},
				RegexOptions.IgnoreCase
			);

			// Log level (with optional coloring)
			string levelString = logLevel.ToString();
			if (LoggerSetup.Settings.ColorLogLevel)
			{
				string color = GetColorForLevel(logLevel);

				if (!string.IsNullOrWhiteSpace(color))
				{
					levelString = $"<color={color}>{levelString}</color>";
				}
			}
			result = result.Replace("{level}", levelString);

			// Category
			result = result.Replace("{category}", category ?? string.Empty);

			// Message
			result = result.Replace("{message}", message ?? string.Empty);

			// Thread ID
			if (result.Contains("{threadId}"))
			{
				result = result.Replace("{threadId}", Thread.CurrentThread.ManagedThreadId.ToString());
			}

			// Task ID
			if (result.Contains("{taskId}"))
			{
				result = result.Replace("{taskId}", Task.CurrentId?.ToString() ?? "null");
			}

			return result;
		}

		private static string GetColorForLevel(LogLevel level)
		{
			if (!LoggerSetup.Settings.ColorLogLevel)
			{
				return string.Empty;
			}

			return level switch
			{
				LogLevel.Trace => LoggerSetup.Settings.TraceColor,
				LogLevel.Debug => LoggerSetup.Settings.DebugColor,
				LogLevel.Information => LoggerSetup.Settings.InformationColor,
				LogLevel.Warning => LoggerSetup.Settings.WarningColor,
				LogLevel.Error => LoggerSetup.Settings.ErrorColor,
				LogLevel.Critical => LoggerSetup.Settings.CriticalColor,
				_ => LoggerSetup.Settings.NoneColor
			};
		}
	}
}