using System;
using SubtitleBytesClearFormatting.Cleaners;

namespace SubtitleFileCleanerGUI.Attributes
{
    // Stores metadata about the subtitle cleaner type
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SubtitleCleanerAttribute : Attribute
    {
        private readonly Type subtitleCleaner;

        public SubtitleCleanerAttribute(Type subtitleCleaner)
        {
            this.subtitleCleaner = subtitleCleaner;
        }

        public ISubtitleCleanerAsync GetAsyncCleaner()
        {
            object instance = Activator.CreateInstance(subtitleCleaner);
            return (ISubtitleCleanerAsync)instance;
        }
    }
}
