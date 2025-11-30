// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using System.IO;
using UnityEngine;

namespace Helyn.Logger
{
	public static class LoggerSettingsLoader
	{
		private static string SettingsBasePath => Path.Combine(Application.streamingAssetsPath, "HelynLogger");
		private const string settingsFileName = "HelynLoggerSettings.json";
		private static string SettingsFilePath => System.IO.Path.Combine(SettingsBasePath, settingsFileName);

		public static LoggerSettings LoadSettings()
		{
			if (!System.IO.File.Exists(SettingsFilePath))
			{
				SaveSettings(null);
			}

			try
			{
				string json = System.IO.File.ReadAllText(SettingsFilePath);
				LoggerSettings settings = JsonUtility.FromJson<LoggerSettings>(json);
				return settings ?? new LoggerSettings();
			}
			catch (System.Exception ex)
			{
				Debug.LogError($"Error loading logger settings from {SettingsFilePath}: {ex.Message}. Using default settings.");
				return new LoggerSettings();
			}
		}

		public static void SaveSettings(LoggerSettings settings)
		{
			settings ??= new LoggerSettings();

			try
			{
				string json = JsonUtility.ToJson(settings, true);
				System.IO.File.WriteAllText(SettingsFilePath, json);
			}
			catch (System.Exception ex)
			{
				Debug.LogError($"Error saving logger settings to {SettingsFilePath}: {ex.Message}");
			}
		}
	}
}