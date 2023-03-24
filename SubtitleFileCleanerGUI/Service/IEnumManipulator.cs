using System;
using System.Collections.Generic;

namespace SubtitleFileCleanerGUI.Service
{
    public interface IEnumManipulator
    {
        public IEnumerable<TEnum> GetAllEnumValues<TEnum>() where TEnum: struct, Enum;
    }
}
