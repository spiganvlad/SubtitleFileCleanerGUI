using System;

namespace SubtitleFileCleanerGUI.Domain.Attributes
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

        public string GetTagsMethodName()
        {
            return methodName;
        }
    }
}
