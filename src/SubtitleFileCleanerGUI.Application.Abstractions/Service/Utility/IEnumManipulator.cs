using System;
using System.Collections.Generic;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility
{
    public interface IEnumManipulator
    {
        public IEnumerable<TEnum> GetAllEnumValues<TEnum>() where TEnum : struct, Enum;
    }
}
