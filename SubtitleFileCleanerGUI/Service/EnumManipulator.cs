using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SubtitleFileCleanerGUI.Service
{
    public static class EnumManipulator<TEnum> where TEnum : struct, Enum
    {
        public static IEnumerable<TAttribute> GetEnumAttributes<TAttribute>(TEnum enumValue) where TAttribute : Attribute
        {
            Type enumType = typeof(TEnum);
            MemberInfo[] memberInfos = enumType.GetMember(enumValue.ToString());
            return memberInfos[0].GetCustomAttributes<TAttribute>(false);
        }

        public static IEnumerable<TEnum> GetAllEnumValues() => Enum.GetValues<TEnum>().Cast<TEnum>();
    }
}
