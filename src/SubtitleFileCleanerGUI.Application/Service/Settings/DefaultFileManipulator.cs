﻿using System.Linq;
using Microsoft.Extensions.Configuration;
using SubtitleFileCleanerGUI.Application.Abstractions.Enums;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Settings;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Domain.Attributes;
using SubtitleFileCleanerGUI.Domain.Enums;
using SubtitleFileCleanerGUI.Domain.Model;

namespace SubtitleFileCleanerGUI.Application.Service.Settings
{
    public class DefaultFileManipulator : IDefaultFileManipulator
    {
        private readonly IConfiguration configuration;
        private readonly IAttributeManipulator attributeManipulator;

        public DefaultFileManipulator(IConfiguration configuration, IAttributeManipulator attributeManipulator)
        {
            this.configuration = configuration;
            this.attributeManipulator = attributeManipulator;
        }

        public SubtitleFile GetDefaultFile(DefaultFileTypes fileType)
        {
            var attributes = attributeManipulator.GetAttributes<DefaultFileTypes, SinglePathAttribute>(fileType);
            var path = attributes.First().Path;

            var destinationPath = configuration.GetValue<string>($"{path}:PathDestination");
            var cleaner = configuration.GetValue<SubtitleCleaners>($"{path}:SubtitleConverter");
            var deleteTags = configuration.GetValue<bool>($"{path}:DeleteTagsOption");
            var toOneLine = configuration.GetValue<bool>($"{path}:ToOneLineOption");

            return new SubtitleFile { PathDestination = destinationPath, Cleaner = cleaner, DeleteTags = deleteTags, ToOneLine = toOneLine };
        }

        public void SetDefaultFile(SubtitleFile file, DefaultFileTypes fileTypes)
        {
            var attributes = attributeManipulator.GetAttributes<DefaultFileTypes, SinglePathAttribute>(fileTypes);
            var path = attributes.First().Path;

            configuration[$"{path}:PathDestination"] = file.PathDestination;
            configuration[$"{path}:SubtitleConverter"] = file.Cleaner.ToString();
            configuration[$"{path}:DeleteTagsOption"] = file.DeleteTags.ToString();
            configuration[$"{path}:ToOneLineOption"] = file.ToOneLine.ToString();
        }
    }
}
