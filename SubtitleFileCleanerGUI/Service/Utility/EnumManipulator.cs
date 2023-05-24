using System;
using System.Collections.Generic;
using System.Linq;

namespace SubtitleFileCleanerGUI.Service.Utility
{
    public class EnumManipulator : IEnumManipulator
    {
        public IEnumerable<TEnum> GetAllEnumValues<TEnum>() where TEnum : struct, Enum =>
            Enum.GetValues<TEnum>().Cast<TEnum>();
    }
}
