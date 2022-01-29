using System;
using System.Reflection;
using System.Collections.Generic;

namespace SubtitleFileCleanerGUI.Service
{
    public static class EnumAttributeManipulator<TEnum> where TEnum : Enum
    {
        public static IEnumerable<TAttribute> GetEnumAttributes<TAttribute>(TEnum enumValue) where TAttribute : Attribute
        {
            Type enumType = typeof(TEnum);
            MemberInfo[] memberInfos = enumType.GetMember(enumValue.ToString());
            return memberInfos[0].GetCustomAttributes<TAttribute>(false);
        }
    }
}
