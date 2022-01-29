using System;
using SubtitleBytesClearFormatting.Cleaner;

namespace SubtitleFileCleanerGUI.Model
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

        public ISubtitleCleanerAsync GetAsyncCleaner(byte[] b)
        {
            object instance = Activator.CreateInstance(subtitleCleaner, b);
            return (ISubtitleCleanerAsync)instance;
        }
    }
}
