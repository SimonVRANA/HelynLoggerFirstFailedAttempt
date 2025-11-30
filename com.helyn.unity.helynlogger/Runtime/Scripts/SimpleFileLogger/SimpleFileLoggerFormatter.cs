// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Helyn.Logger
{
	public static class SimpleFileLoggerFormatter
	{
		public static string ApplyFormat(string rawMessage,
										 LogLevel logLevel,
										 string category)
		{
			if (string.IsNullOrWhiteSpace(LoggerSetup.Settings.FileLogFormat))
			{
				return rawMessage;
			}

			string result = LoggerSetup.Settings.FileLogFormat;

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

			// --- Level ---
			result = result.Replace("{level}", logLevel.ToString());

			// --- Category ---
			result = result.Replace("{category}", category);

			// --- Message ---
			result = result.Replace("{message}", rawMessage);

			// --- ThreadId ---
			result = result.Replace("{threadId}", Environment.CurrentManagedThreadId.ToString());

			// --- TaskId ---
			int? taskId = Task.CurrentId;
			result = result.Replace("{taskId}", taskId?.ToString() ?? "none");

			return result;
		}
	}
}