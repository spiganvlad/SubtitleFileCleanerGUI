using System;

namespace SubtitleFileCleanerGUI.Attributes
{
    // Stores metadata about file path (single means one attribute per field)
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SinglePathAttribute : Attribute
    {
        public string Path { get; private set; }

        public SinglePathAttribute(string path)
        {
            Path = path;
        }
    }
}
