using MelonLoader;
using MelonLoader.Preferences;

namespace MabelTheCat
{
	public class Preferences
	{
		private const string CONFIG_FILE = "config.cfg";
		private const string USER_DATA = "UserData/MabelTheCat/";
        internal static Dictionary<MelonPreferences_Entry, object> LastSavedValues = new();

        internal static MelonPreferences_Category MabelTheCatCategory;
		internal static MelonPreferences_Entry<bool> showAllCats;

        internal static void InitPrefs()
		{
			if (!Directory.Exists(USER_DATA)) { Directory.CreateDirectory(USER_DATA); }

            //General settings
            MabelTheCatCategory = MelonPreferences.CreateCategory("MabelTheCat", "Settings");
            MabelTheCatCategory.SetFilePath(Path.Combine(USER_DATA, CONFIG_FILE));

            showAllCats = MabelTheCatCategory.CreateEntry("showAllCats", false, "Show all Cats", "Toggling ON will have all the Cats Shown.");

            StoreLastSavedPrefs();
		}

		internal static void StoreLastSavedPrefs()
		{
			List<MelonPreferences_Entry> prefs = new();
			prefs.AddRange(MabelTheCatCategory.Entries);

			foreach (MelonPreferences_Entry entry in  prefs) { LastSavedValues[entry] = entry.BoxedValue; }
		}

		public static bool AnyPrefsChanged()
		{
			foreach (KeyValuePair<MelonPreferences_Entry, object> pair in LastSavedValues)
			{
				if (!pair.Key.BoxedValue.Equals(pair.Value)) { return true; }
			}
			return false;
		}

		public static bool IsPrefChanged(MelonPreferences_Entry entry)
		{
			if (LastSavedValues.TryGetValue(entry, out object? lastValue)) { return !entry.BoxedValue.Equals(lastValue); }
			return false;
		}
	}
}