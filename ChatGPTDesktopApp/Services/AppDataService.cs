using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Windows.Storage;

namespace ChatGPTDesktopApp.Services;

internal static class AppDataService
{
    private static readonly ApplicationDataContainer LocalAppSettings = ApplicationData.GetDefault().LocalSettings;

    /// <summary>
    /// Adds or updates a value in the local application settings.
    /// </summary>
    /// <param name="key">The key of the setting to add or update.</param>
    /// <param name="value">The value to set.</param>
    public static void AddOrUpdateValue(string key, object value)
    {
        LocalAppSettings.Values[key] = value;
    }

    /// <summary>
    /// Adds a value to the local application settings only if the key does not already exist.
    /// </summary>
    /// <param name="key">The key of the setting to add.</param>
    /// <param name="value">The value to set.</param>
    public static void AddIfNotExists(string key, object value)
    {
        if (!LocalAppSettings.Values.ContainsKey(key))
        {
            LocalAppSettings.Values[key] = value;
        }
    }

    /// <summary>
    /// Removes a value from the local application settings.
    /// </summary>
    /// <param name="key">The key of the setting to remove.</param>
    public static void RemoveValue(string key)
    {
        LocalAppSettings.Values.Remove(key);
    }

    /// <summary>
    /// Retrieves a value from the local application settings.
    /// </summary>
    /// <typeparam name="T">The type of the setting value.</typeparam>
    /// <param name="key">The key of the setting to retrieve.</param>
    /// <returns>The value of the setting, or null if it does not exist.</returns>
    public static T? GetValue<T>(string key)
    {
        if (LocalAppSettings.Values.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default;
    }

    /// <summary>
    /// Checks if a key exists in the local application settings.
    /// </summary>
    /// <param name="key">The key of the setting to check.</param>
    /// <returns>True if the setting exists; otherwise, false.</returns>
    public static bool ContainsKey(string key)
    {
        return LocalAppSettings.Values.ContainsKey(key);
    }

    /// <summary>
    /// Clears all values from the local application settings.
    /// </summary>
    public static void ClearAllValues()
    {
        LocalAppSettings.Values.Clear();
    }

    /// <summary>
    /// Retrieves all configuration keys currently stored in the local application settings.
    /// </summary>
    /// <returns>An enumerable collection of strings containing the names of all keys present in the local application settings.
    /// The collection will be empty if no keys are defined.</returns>
    public static IEnumerable<string> GetAllKeys()
    {
        return LocalAppSettings.Values.Keys;
    }

    /// <summary>
    /// Gets the count of values stored in the local application settings.
    /// </summary>
    /// <returns>The count of values stored in the local application settings.</returns>
    public static int GetValuesCount()
    {
        return LocalAppSettings.Values.Count;
    }
}
