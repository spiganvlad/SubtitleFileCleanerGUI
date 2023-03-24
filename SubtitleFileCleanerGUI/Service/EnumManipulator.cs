using System;
using System.Linq;
using System.Collections.Generic;

namespace SubtitleFileCleanerGUI.Service
{
    public class EnumManipulator : IEnumManipulator
    {
        public IEnumerable<TEnum> GetAllEnumValues<TEnum>() where TEnum : struct, Enum =>
            Enum.GetValues<TEnum>().Cast<TEnum>();
    }
}
