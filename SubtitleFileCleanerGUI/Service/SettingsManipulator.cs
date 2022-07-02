using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SubtitleFileCleanerGUI.Model;

namespace SubtitleFileCleanerGUI.Service
{
    public static class SettingsManipulator
    {
        // Used to auto-update settings
        private delegate void SettingsChangedEventHandler(ICustomSettings settings, SettingsTypes type);
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

        public static void SaveSettings(ICustomSettings settings, SettingsTypes settingsType)
        {
            JObject json = JObject.FromObject(settings);

            var attributes = EnumAttributeManipulator<SettingsTypes>.GetEnumAttributes<SinglePathAttribute>(settingsType);
            using FileStream stream = new(attributes.First().Path, FileMode.OpenOrCreate, FileAccess.Write);
            using StreamWriter file = new(stream);
            using JsonTextWriter writer = new(file);

            json.WriteTo(writer);
            SettingsChanged?.Invoke(settings, settingsType);
        }
    }
}
