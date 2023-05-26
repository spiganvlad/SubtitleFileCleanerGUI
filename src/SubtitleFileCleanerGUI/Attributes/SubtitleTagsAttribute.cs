using System;
using System.Collections.Generic;
using SubtitleBytesClearFormatting.TagsGenerate;

namespace SubtitleFileCleanerGUI.Attributes
{
    // Stores metadata about the method for getting tags
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SubtitleTagsAttribute : Attribute
    {
        private readonly string methodName;

        public SubtitleTagsAttribute(string methodName)
        {
            this.methodName = methodName;
        }

        public Dictionary<byte, List<TxtTag>> GetSubtitleTagsDictionary()
        {
            Type type = typeof(TagsCollectionGeneretor);
            return (Dictionary<byte, List<TxtTag>>)type.GetMethod(methodName).Invoke(null, null);
        }
    }
}
