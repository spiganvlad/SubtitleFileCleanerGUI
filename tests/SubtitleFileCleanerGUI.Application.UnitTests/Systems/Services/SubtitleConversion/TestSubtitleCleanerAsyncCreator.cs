using System;
using System.Linq;
using FluentAssertions;
using Moq;
using SubtitleBytesClearFormatting.Cleaners;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Application.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Application.UnitTests.Helpers.TestObjects.SubtitleAsyncCleaners;
using SubtitleFileCleanerGUI.Domain.Attributes;
using SubtitleFileCleanerGUI.Domain.Enums;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.SubtitleConversion
{
    public class TestSubtitleCleanerAsyncCreator
    {
        private readonly Mock<IAttributeManipulator> attributeManipulatorMock;

        public TestSubtitleCleanerAsyncCreator()
        {
            attributeManipulatorMock = new Mock<IAttributeManipulator>();
        }

        [Fact]
        public void Create_WithSupportedCleanerType_ReturnSubtitleCleanerAsync()
        {
            // Arrange
            var cleanerEnum = (SubtitleCleaners)(-1);

            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleCleanerAsyncTypeAttribute>(cleanerEnum))
                .Returns(new[] { new SubtitleCleanerAsyncTypeAttribute(typeof(TestSubtitleCleanerAsyncEmptyConstructor)) });

            var subtitleCleanerCreator = new SubtitleAsyncCleanerCreator(attributeManipulatorMock.Object);

            // Act
            var result = subtitleCleanerCreator.Create(cleanerEnum);

            // Assert
            attributeManipulatorMock.Verify(am => am.GetAttributes<SubtitleCleaners, SubtitleCleanerAsyncTypeAttribute>(It.IsAny<SubtitleCleaners>()),
                Times.Once());

            result.Should().NotBeNull()
                .And.BeOfType<TestSubtitleCleanerAsyncEmptyConstructor>();
        }

        [Fact]
        public void Create_WithUnsupportedCleanerType_ThrowInvalidOperationException()
        {
            // Arrange
            var cleanerEnum = (SubtitleCleaners)(-1);

            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleCleanerAsyncTypeAttribute>(cleanerEnum))
                .Returns(Enumerable.Empty<SubtitleCleanerAsyncTypeAttribute>());

            var subtitleCleanerCreator = new SubtitleAsyncCleanerCreator(attributeManipulatorMock.Object);

            // Act
            Action act = () => { subtitleCleanerCreator.Create(cleanerEnum); };

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("No async subtitle cleaners found for cleaner type: -1.");

            attributeManipulatorMock.Verify(am =>
                am.GetAttributes<SubtitleCleaners, SubtitleCleanerAsyncTypeAttribute>(It.IsAny<SubtitleCleaners>()), Times.Once());
        }

        [Fact]
        public void Create_WithNoParameterlessConstructor_ThrowInvalidOperationException()
        {
            // Arrange
            var cleanerEnum = (SubtitleCleaners)(-1);

            var parameterlessConstructorCleanerType = typeof(TestSubtitleCleanerAsyncByteConstructor);
            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleCleanerAsyncTypeAttribute>(cleanerEnum))
                .Returns(new[] { new SubtitleCleanerAsyncTypeAttribute(parameterlessConstructorCleanerType) });

            var subtitleCleanerCreator = new SubtitleAsyncCleanerCreator(attributeManipulatorMock.Object);

            // Act
            Action act = () => { subtitleCleanerCreator.Create(cleanerEnum); };

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("The subtitle cleaner async instance cannot be created. " +
                $"No parameterless constructor found in subtitle cleaner async \"{parameterlessConstructorCleanerType}\" for cleaner type: -1.");

            attributeManipulatorMock.Verify(am => am.GetAttributes<SubtitleCleaners, SubtitleCleanerAsyncTypeAttribute>(It.IsAny<SubtitleCleaners>()),
                Times.Once());
        }

        [Fact]
        public void Create_WithMismatchedType_ThrowInvalidOperationException()
        {
            // Arrange
            var cleanerEnum = (SubtitleCleaners)(-1);

            var mismatchedType = GetType();
            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleCleanerAsyncTypeAttribute>(cleanerEnum))
                .Returns(new[] { new SubtitleCleanerAsyncTypeAttribute(mismatchedType) });

            var subtitleCleanerCreator = new SubtitleAsyncCleanerCreator(attributeManipulatorMock.Object);

            // Act
            Action act = () => { subtitleCleanerCreator.Create(cleanerEnum); };

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage($"The instantiated instance of type \"{mismatchedType}\" " +
                    $"is not derived from \"{typeof(ISubtitleCleanerAsync)}\" for cleaner type: -1.");

            attributeManipulatorMock.Verify(am =>
                am.GetAttributes<SubtitleCleaners, SubtitleCleanerAsyncTypeAttribute>(It.IsAny<SubtitleCleaners>()), Times.Once());
        }
    }
}
