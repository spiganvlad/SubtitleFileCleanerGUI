using System;
using FluentAssertions;
using Moq;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Application.Service.SubtitleConversion;
using SubtitleFileCleanerGUI.Domain.Attributes;
using SubtitleFileCleanerGUI.Domain.Enums;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.SubtitleConversion
{
    public class TestAutoCleanerDefiner
    {
        private readonly Mock<IEnumManipulator> enumManipulatorMock;
        private readonly Mock<IAttributeManipulator> attributeManipulatorMock;

        public TestAutoCleanerDefiner()
        {
            enumManipulatorMock = new Mock<IEnumManipulator>();
            attributeManipulatorMock = new Mock<IAttributeManipulator>();
        }

        [Fact]
        public void Define_WithSupportedExtension_ReturnSubtitleCleaner()
        {
            // Arrange
            var extension = ".test";

            var firstCleaner = (SubtitleCleaners)(-1);
            var secondCleaner = (SubtitleCleaners)(-2);

            enumManipulatorMock.Setup(em => em.GetAllEnumValues<SubtitleCleaners>())
                .Returns(new[] { firstCleaner, secondCleaner });

            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleExtensionAttribute>(firstCleaner))
                .Returns(new[] { new SubtitleExtensionAttribute(".notTest") });

            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleExtensionAttribute>(secondCleaner))
                .Returns(new[] { new SubtitleExtensionAttribute(".test") });

            var autoCleanerDefiner = new AutoCleanerDefiner(enumManipulatorMock.Object, attributeManipulatorMock.Object);

            // Act
            var result = autoCleanerDefiner.Define(extension);

            // Assert
            enumManipulatorMock.Verify(em => em.GetAllEnumValues<SubtitleCleaners>(), Times.Once());
            attributeManipulatorMock.Verify(am => am.GetAttributes<SubtitleCleaners, SubtitleExtensionAttribute>(It.IsAny<SubtitleCleaners>()),
                Times.Exactly(2));

            result.Should().Be(secondCleaner);
        }

        [Fact]
        public void Define_WithTwoCleanersSupportingOneExtension_ReturnFirstSubtitleCleaner()
        {
            // Arrange
            var extension = ".test";

            var firstCleaner = (SubtitleCleaners)(-1);
            var secondCleaner = (SubtitleCleaners)(-2);

            enumManipulatorMock.Setup(em => em.GetAllEnumValues<SubtitleCleaners>())
                .Returns(new[] { firstCleaner, secondCleaner });

            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleExtensionAttribute>(firstCleaner))
                .Returns(new[] { new SubtitleExtensionAttribute(".test") });

            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleExtensionAttribute>(secondCleaner))
                .Returns(new[] { new SubtitleExtensionAttribute(".test") });

            var autoCleanerDefiner = new AutoCleanerDefiner(enumManipulatorMock.Object, attributeManipulatorMock.Object);

            // Act
            var result = autoCleanerDefiner.Define(extension);

            // Assert
            enumManipulatorMock.Verify(em => em.GetAllEnumValues<SubtitleCleaners>(), Times.Once());
            attributeManipulatorMock.Verify(am => am.GetAttributes<SubtitleCleaners, SubtitleExtensionAttribute>(It.IsAny<SubtitleCleaners>()),
                Times.Once());

            result.Should().Be(firstCleaner);
        }

        [Fact]
        public void Define_WithCleanerSupportingMultipleExtensions_ReturnSubtitleCleaner()
        {
            // Arrange
            var extension = ".test";

            var firstCleaner = (SubtitleCleaners)(-1);
            var secondCleaner = (SubtitleCleaners)(-2);

            enumManipulatorMock.Setup(em => em.GetAllEnumValues<SubtitleCleaners>())
                .Returns(new[] { firstCleaner, secondCleaner });

            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleExtensionAttribute>(firstCleaner))
                .Returns(new[] { new SubtitleExtensionAttribute(".notTest") });

            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleExtensionAttribute>(secondCleaner))
                .Returns(new[] { 
                    new SubtitleExtensionAttribute(".notTest"), 
                    new SubtitleExtensionAttribute(".fooTest"),
                    new SubtitleExtensionAttribute(".test")
                });

            var autoCleanerDefiner = new AutoCleanerDefiner(enumManipulatorMock.Object, attributeManipulatorMock.Object);

            // Act
            var result = autoCleanerDefiner.Define(extension);

            // Assert
            enumManipulatorMock.Verify(em => em.GetAllEnumValues<SubtitleCleaners>(), Times.Once());
            attributeManipulatorMock.Verify(am => am.GetAttributes<SubtitleCleaners, SubtitleExtensionAttribute>(It.IsAny<SubtitleCleaners>()),
                Times.Exactly(2));

            result.Should().Be(secondCleaner);

        }

        [Fact]
        public void Define_WithUnsupportedExtension_ThrowInvalidOperationException()
        {
            // Arrange
            var extension = ".test";

            var firstCleaner = (SubtitleCleaners)(-1);
            var secondCleaner = (SubtitleCleaners)(-2);

            enumManipulatorMock.Setup(em => em.GetAllEnumValues<SubtitleCleaners>())
                .Returns(new[] { firstCleaner, secondCleaner });

            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleExtensionAttribute>(firstCleaner))
                .Returns(new[] { new SubtitleExtensionAttribute(".notTest") });

            attributeManipulatorMock.Setup(am => am.GetAttributes<SubtitleCleaners, SubtitleExtensionAttribute>(secondCleaner))
                .Returns(new[] { new SubtitleExtensionAttribute(".fooTest") });

            var autoCleanerDefiner = new AutoCleanerDefiner(enumManipulatorMock.Object, attributeManipulatorMock.Object);

            // Act
            Action act = () => { autoCleanerDefiner.Define(extension); };

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("Unable to define converter for .test extension.");

            enumManipulatorMock.Verify(em => em.GetAllEnumValues<SubtitleCleaners>(), Times.Once());
            attributeManipulatorMock.Verify(am => am.GetAttributes<SubtitleCleaners, SubtitleExtensionAttribute>(It.IsAny<SubtitleCleaners>()),
                Times.Exactly(2));
        }
    }
}
