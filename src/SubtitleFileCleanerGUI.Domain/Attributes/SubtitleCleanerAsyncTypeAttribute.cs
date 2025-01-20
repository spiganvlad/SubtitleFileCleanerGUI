using System;

namespace SubtitleFileCleanerGUI.Domain.Attributes
{
    // Stores metadata about the subtitle cleaner type
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SubtitleCleanerAsyncTypeAttribute : Attribute
    {
        private readonly Type subtitleCleanerAsyncType;

        public SubtitleCleanerAsyncTypeAttribute(Type subtitleCleanerAsyncType)
        {
            this.subtitleCleanerAsyncType = subtitleCleanerAsyncType;
        }

        public Type GetAsyncCleanerType()
        {
            return subtitleCleanerAsyncType;
        }
    }
}
