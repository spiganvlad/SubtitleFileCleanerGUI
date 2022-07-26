using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SubtitleFileCleanerGUI.Model;

namespace SubtitleFileCleanerGUI.Service
{
    // Supported settings types
    public enum DefaultFileTypes
    {
        [SinglePath("./DefaultFiles/customFile.json")]
        Custom,
        [SinglePath("./DefaultFiles/defaultFile.json")]
        Default
    }

    public static class DefaultFilesManipulator
    {
        public static T LoadDefaultFile<T>(DefaultFileTypes defaultFileTypes) where T : SubtitleFile
        {
            var attribute = EnumManipulator<DefaultFileTypes>.GetEnumAttributes<SinglePathAttribute>(defaultFileTypes);
            using FileStream stream = new(attribute.First().Path, FileMode.Open, FileAccess.Read);
            using StreamReader file = new(stream);
            using JsonTextReader reader = new(file);

            JObject json = (JObject)JToken.ReadFrom(reader);

            return json.ToObject<T>();
        }

        public static void SaveDefaultFile(SubtitleFile defaultFile, DefaultFileTypes defaultFileType)
        {
            JObject json = JObject.FromObject(new
            {
                defaultFile.PathDestination,
                defaultFile.Cleaner,
                defaultFile.DeleteTags,
                defaultFile.ToOneLine
            });

            var attributes = EnumManipulator<DefaultFileTypes>.GetEnumAttributes<SinglePathAttribute>(defaultFileType);
            using FileStream stream = new(attributes.First().Path, FileMode.OpenOrCreate, FileAccess.Write);
            using StreamWriter file = new(stream);
            using JsonTextWriter writer = new(file);

            json.WriteTo(writer);
        }
    }
}
