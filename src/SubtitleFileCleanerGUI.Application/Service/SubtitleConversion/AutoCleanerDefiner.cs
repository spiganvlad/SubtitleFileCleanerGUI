﻿using System;
using System.Linq;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Domain.Attributes;
using SubtitleFileCleanerGUI.Domain.Enums;

namespace SubtitleFileCleanerGUI.Application.Service.SubtitleConversion
{
    public class AutoCleanerDefiner : IAutoCleanerDefiner
    {
        private readonly IEnumManipulator enumManipulator;
        private readonly IAttributeManipulator attributeManipulator;

        public AutoCleanerDefiner(IEnumManipulator enumManipulator, IAttributeManipulator attributeManipulator)
        {
            this.enumManipulator = enumManipulator;
            this.attributeManipulator = attributeManipulator;
        }

        public SubtitleCleaners Define(string fileExtension)
        {
            var cleaners = enumManipulator.GetAllEnumValues<SubtitleCleaners>();

            foreach (var cleaner in cleaners)
            {
                var attributes = attributeManipulator.GetAttributes<SubtitleCleaners, SubtitleExtensionAttribute>(cleaner);

                if (!attributes.Any())
                    continue;

                foreach (var attribute in attributes)
                {
                    if (attribute.Extension == fileExtension)
                        return cleaner;
                }
            }

            throw new InvalidOperationException($"Unable to define converter for {fileExtension} extension.");
        }
    }
}
