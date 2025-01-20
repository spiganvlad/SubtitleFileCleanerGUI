using System;
using System.Linq;
using FluentAssertions;
using Moq;
using SubtitleBytesClearFormatting.TagsGenerate;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Application.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Domain.Attributes;
using SubtitleFileCleanerGUI.Domain.Enums;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.SubtitleConversion
{
    public class TestTagCollectionCreator
    {
        private readonly Mock<IAttributeManipulator> attributeManipulatorMock;

        public TestTagCollectionCreator()
        {
            attributeManipulatorMock = new Mock<IAttributeManipulator>();
        }

        [Fact]
        public void Create_WithGetBasicTags_ReturnDirectory()
        {
            // Arrange
            var cleanerEnum = (SubtitleCleaners)(-1);

            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleTagsAttribute>(cleanerEnum))
                .Returns(new[] { new SubtitleTagsAttribute(nameof(TagsCollectionGeneretor.GetBasicTags)) });

            var tagCollectionCreator = new TagCollectionCreator(attributeManipulatorMock.Object);

            // Act
            var result = tagCollectionCreator.Create(cleanerEnum);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Create_WithUnsupportedTagCollection_ThrowInvalidOperationException()
        {
            // Arrange
            var cleanerEnum = (SubtitleCleaners)(-1);

            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleTagsAttribute>(cleanerEnum))
                .Returns(Enumerable.Empty<SubtitleTagsAttribute>());

            var tagCollectionCreator = new TagCollectionCreator(attributeManipulatorMock.Object);

            // Act
            Action act = () => { tagCollectionCreator.Create(cleanerEnum); };

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("No subtitle tags collection found for cleaner type: -1");

            attributeManipulatorMock.Verify(am => am.GetAttributes<SubtitleCleaners, SubtitleTagsAttribute>(It.IsAny<SubtitleCleaners>()),
                Times.Once());
        }

        [Fact]
        public void Create_WithNonExistentMethod_ThrowInvalidOperationException()
        {
            // Arrange
            var cleanerEnum = (SubtitleCleaners)(-1);

            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleTagsAttribute>(cleanerEnum))
                .Returns(new[] { new SubtitleTagsAttribute("TestFoo") });

            var tagCollectionCreator = new TagCollectionCreator(attributeManipulatorMock.Object);

            // Act
            Action act = () => { tagCollectionCreator.Create(cleanerEnum); };

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage($"Method \"TestFoo\" does not exist on type " +
                    $"\"{typeof(TagsCollectionGeneretor)}\" for cleaner type: -1.");

            attributeManipulatorMock.Verify(am => am.GetAttributes<SubtitleCleaners, SubtitleTagsAttribute>(It.IsAny<SubtitleCleaners>()),
                Times.Once());
        }
    }
}
