﻿using System;
using System.Collections.Generic;

namespace SubtitleFileCleanerGUI.Service
{
    public interface IAttributeManipulator
    {
        public IEnumerable<TAttribute> GetAttributes<TEnum, TAttribute>(TEnum enumValue)
            where TEnum: struct, Enum where TAttribute: Attribute;
    }
}