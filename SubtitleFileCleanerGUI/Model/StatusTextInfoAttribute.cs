using System;

namespace SubtitleFileCleanerGUI.Model
{
    // Stores metadata about the status type description
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class StatusTextInfoAttribute : Attribute
    {
        public string TextInfo { get; private set; }

        public StatusTextInfoAttribute(string textInfo) => TextInfo = textInfo;
    }
}
