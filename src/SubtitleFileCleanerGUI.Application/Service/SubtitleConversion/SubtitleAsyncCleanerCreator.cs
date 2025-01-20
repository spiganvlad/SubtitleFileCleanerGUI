using System;
using System.Linq;
using SubtitleBytesClearFormatting.Cleaners;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Domain.Attributes;
using SubtitleFileCleanerGUI.Domain.Enums;

namespace SubtitleFileCleanerGUI.Application.Service.SubtitleConversion
{
    public class SubtitleAsyncCleanerCreator : ISubtitleAsyncCleanerCreator
    {
        private readonly IAttributeManipulator attributeManipulator;

        public SubtitleAsyncCleanerCreator(IAttributeManipulator attributeManipulator)
        {
            this.attributeManipulator = attributeManipulator;
        }

        public ISubtitleCleanerAsync Create(SubtitleCleaners cleaner)
        {
            var attributes = attributeManipulator.GetAttributes<SubtitleCleaners, SubtitleCleanerAsyncTypeAttribute>(cleaner);
            if (!attributes.Any())
                throw new InvalidOperationException($"No async subtitle cleaners found for cleaner type: {cleaner}.");

            var cleanerType = attributes.First().GetAsyncCleanerType();
            if (cleanerType.GetConstructor(Type.EmptyTypes) == null)
            {
                throw new InvalidOperationException("The subtitle cleaner async instance cannot be created. " +
                    $"No parameterless constructor found in subtitle cleaner async \"{cleanerType}\" for cleaner type: {cleaner}.");
            }

            if (Activator.CreateInstance(cleanerType) is not ISubtitleCleanerAsync cleanerInstance)
            {
                throw new InvalidOperationException($"The instantiated instance of type \"{cleanerType}\" " +
                    $"is not derived from \"{typeof(ISubtitleCleanerAsync)}\" for cleaner type: {cleaner}.");
            }

            return cleanerInstance;
        }
    }
}
