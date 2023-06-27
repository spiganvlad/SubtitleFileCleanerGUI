using System;
using System.Collections.Generic;
using System.Reflection;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;

namespace SubtitleFileCleanerGUI.Application.Service.Utility
{
    public class AttributeManipulator : IAttributeManipulator
    {
        public IEnumerable<TAttribute> GetAttributes<TEnum, TAttribute>(TEnum enumValue)
            where TEnum : struct, Enum where TAttribute : Attribute
        {
            var enumType = typeof(TEnum);
            var memberInfos = enumType.GetMember(enumValue.ToString());
            return memberInfos[0].GetCustomAttributes<TAttribute>(false);
        }
    }
}
