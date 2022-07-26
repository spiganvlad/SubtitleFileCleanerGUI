﻿using System;

namespace SubtitleFileCleanerGUI.Model
{
    // Stores metadata about subtitle file extensions
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class SubtitleExtensionAttribute : Attribute
    {
        public string Extension { get; private set; }

        public SubtitleExtensionAttribute(string extension)
        {
            Extension = extension;
        }
    }
}
