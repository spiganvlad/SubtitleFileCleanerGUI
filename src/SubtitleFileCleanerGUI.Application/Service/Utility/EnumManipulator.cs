using System;
using System.Collections.Generic;
using System.Linq;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;

namespace SubtitleFileCleanerGUI.Application.Service.Utility
{
    public class EnumManipulator : IEnumManipulator
    {
        public IEnumerable<TEnum> GetAllEnumValues<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetValues<TEnum>().Cast<TEnum>();
        } 
    }
}
