using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SubtitleFileCleanerGUI.Model;

namespace SubtitleFileCleanerGUI.Service
{
    // Supported settings types
    public enum SettingsTypes
    {
        [SinglePath("./DefaultFiles/customFile.json")]
        Custom,
        [SinglePath("./DefaultFiles/defaultFile.json")]
        Default
    }

    public static class DefaultFilesManipulator
    {
        public static SubtitleFile LoadSettings(SettingsTypes settingsTypes)
        {
            var attribute = EnumManipulator<SettingsTypes>.GetEnumAttributes<SinglePathAttribute>(settingsTypes);
            using FileStream stream = new(attribute.First().Path, FileMode.Open, FileAccess.Read);
            using StreamReader file = new(stream);
            using JsonTextReader reader = new(file);

            JObject json = (JObject)JToken.ReadFrom(reader);

            return json.ToObject<SubtitleFile>();
        }

        public static void SaveSettings(SubtitleFile settings, SettingsTypes settingsType)
        {
            JObject json = JObject.FromObject(new
            {
                settings.PathDestination,
                settings.Cleaner,
                settings.DeleteTags,
                settings.ToOneLine
            });

            var attributes = EnumManipulator<SettingsTypes>.GetEnumAttributes<SinglePathAttribute>(settingsType);
            using FileStream stream = new(attributes.First().Path, FileMode.OpenOrCreate, FileAccess.Write);
            using StreamWriter file = new(stream);
            using JsonTextWriter writer = new(file);

            json.WriteTo(writer);
        }
    }
}
