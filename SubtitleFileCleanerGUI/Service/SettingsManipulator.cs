using System.IO;
using System.Linq;
using SubtitleFileCleanerGUI.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SubtitleFileCleanerGUI.Service
{
    public enum SettingsTypes
    {
        [SinglePath("./Settings/customSettings.json")]
        Custom,
        [SinglePath("./Settings/defaultSettings.json")]
        Default 
    }

    public static class SettingsManipulator
    {
        // Used to auto-update settings
        private delegate void SettingsChangedEventHandler(CustomSettings settings);
        private static event SettingsChangedEventHandler SettingsChanged;

        public static CustomSettings LoadSettings(SettingsTypes settingsTypes, bool updateAutomatically = false)
        {
            var attribute = EnumAttributeManipulator<SettingsTypes>.GetEnumAttributes<SinglePathAttribute>(settingsTypes);
            using FileStream stream = new(attribute.First().Path, FileMode.Open, FileAccess.Read);
            using StreamReader file = new(stream);
            using JsonTextReader reader = new(file);

            JObject json = (JObject)JToken.ReadFrom(reader);

            CustomSettings settings = json.ToObject<CustomSettings>();
            
            if (updateAutomatically)
                SettingsChanged += settings.UpdateSettings;

            return settings;
        }

        public static void SaveSettings(CustomSettings settings, SettingsTypes settingsTypes)
        {
            JObject json = JObject.FromObject(settings);

            var attributes = EnumAttributeManipulator<SettingsTypes>.GetEnumAttributes<SinglePathAttribute>(settingsTypes);
            using FileStream stream = new(attributes.First().Path, FileMode.OpenOrCreate, FileAccess.Write);
            using StreamWriter file = new(stream);
            using JsonTextWriter writer = new(file);

            json.WriteTo(writer);
            UpdateSettings(settings);
        }

        private static void UpdateSettings(CustomSettings settings)
        {
            SettingsChanged?.Invoke(settings);
        }
    }
}
