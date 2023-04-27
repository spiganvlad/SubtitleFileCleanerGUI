using System;
using System.Reflection;
using System.Collections.Generic;

namespace SubtitleFileCleanerGUI.Service.Utility
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
