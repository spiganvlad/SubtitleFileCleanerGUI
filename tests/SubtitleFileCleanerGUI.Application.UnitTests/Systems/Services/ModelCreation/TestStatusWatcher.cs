using System;
using System.Linq;
using FluentAssertions;
using Moq;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Application.Service.ModelCreation;
using SubtitleFileCleanerGUI.Domain.Attributes;
using SubtitleFileCleanerGUI.Domain.Enums;
using SubtitleFileCleanerGUI.Domain.Model;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.ModelCreation
{
    public class TestStatusWatcher
    {
        private readonly Mock<IAttributeManipulator> attributeManipulatorMock;

        public TestStatusWatcher()
        {
            attributeManipulatorMock = new Mock<IAttributeManipulator>();
        }

        [Fact]
        public void Watch_WithStatusTypeChanged_ChangeImageAndInfo()
        {
            // Arrange
            var newStatusType = (StatusTypes)(-2);
            var statusInfo = new StatusInfo
            {
                StatusType = (StatusTypes)(-1),
                ImagePath = @"C:\Test\Image\Path\foo.test",
                TextInfo = "Test info text"
            };

            var newImagePath = @"C:\New\TestImagePath\foo.test";
            attributeManipulatorMock.Setup(am => am.GetAttributes<StatusTypes, SinglePathAttribute>(newStatusType))
                .Returns(new[] { new SinglePathAttribute(newImagePath) });

            var newTextInfo = "New test info text";
            attributeManipulatorMock.Setup(am => am.GetAttributes<StatusTypes, StatusTextInfoAttribute>(newStatusType))
                .Returns(new[] { new StatusTextInfoAttribute(newTextInfo) });

            var statusInfoWatcher = new StatusInfoWatcher(attributeManipulatorMock.Object);
            statusInfoWatcher.Watch(statusInfo);

            // Act
            statusInfo.StatusType = newStatusType;

            // Assert
            attributeManipulatorMock.Verify(am => am.GetAttributes<StatusTypes,SinglePathAttribute>(It.IsAny<StatusTypes>()),
                Times.Once());
            attributeManipulatorMock.Verify(am => am.GetAttributes<StatusTypes, StatusTextInfoAttribute>(It.IsAny<StatusTypes>()),
                Times.Once());

            statusInfo.StatusType.Should().Be(newStatusType);

            statusInfo.ImagePath.Should().NotBeNull()
                .And.Be(newImagePath);

            statusInfo.TextInfo.Should().NotBeNull()
                .And.Be(newTextInfo);
        }

        [Fact]
        public void Watch_WithImagePathChanged_NoMoreChanges()
        {
            // Arrange
            var newImagePath = @"C:\New\TestImagePath\foo.test";

            var statusType = (StatusTypes)(-1);
            var textInfo = "Test info text";
            var statusInfo = new StatusInfo
            {
                StatusType = statusType,
                ImagePath = @"C:\Test\Image\Path\foo.test",
                TextInfo = textInfo
            };

            var statusInfoWatcher = new StatusInfoWatcher(attributeManipulatorMock.Object);
            statusInfoWatcher.Watch(statusInfo);

            // Act
            statusInfo.ImagePath = newImagePath;

            // Assert
            attributeManipulatorMock.Verify(am => am.GetAttributes<StatusTypes, SinglePathAttribute>(It.IsAny<StatusTypes>()),
                Times.Never());
            attributeManipulatorMock.Verify(am => am.GetAttributes<StatusTypes, StatusTextInfoAttribute>(It.IsAny<StatusTypes>()),
                Times.Never());

            statusInfo.StatusType.Should().Be(statusType);

            statusInfo.ImagePath.Should().NotBeNull()
                .And.Be(newImagePath);

            statusInfo.TextInfo.Should().NotBeNull()
                .And.Be(textInfo);
        }

        [Fact]
        public void Watch_WithTextInfoChanged_NoMoreChanges()
        {
            // Arrange
            var newTextInfo = "New test info text";

            var statusType = (StatusTypes)(-1);
            var imagePath = @"C:\Test\Image\Path\foo.test";
            var statusInfo = new StatusInfo
            {
                StatusType = statusType,
                ImagePath = imagePath,
                TextInfo = "Test info text"
            };

            var statusInfoWatcher = new StatusInfoWatcher(attributeManipulatorMock.Object);
            statusInfoWatcher.Watch(statusInfo);

            // Act
            statusInfo.TextInfo = newTextInfo;

            // Assert
            attributeManipulatorMock.Verify(am => am.GetAttributes<StatusTypes, SinglePathAttribute>(It.IsAny<StatusTypes>()),
                Times.Never());
            attributeManipulatorMock.Verify(am => am.GetAttributes<StatusTypes, StatusTextInfoAttribute>(It.IsAny<StatusTypes>()),
                Times.Never());

            statusInfo.StatusType.Should().Be(statusType);

            statusInfo.ImagePath.Should().NotBeNull()
                .And.Be(imagePath);

            statusInfo.TextInfo.Should().NotBeNull()
                .And.Be(newTextInfo);
        }

        [Fact]
        public void Watch_WithNoSinglePathAttribute_ThrowInvalidOperationException()
        {
            // Arrange
            var newStatusType = (StatusTypes)(-2);
            var statusInfo = new StatusInfo { StatusType = (StatusTypes)(-1) };

            attributeManipulatorMock.Setup(am => am.GetAttributes<StatusTypes, SinglePathAttribute>(newStatusType))
                .Returns(Enumerable.Empty<SinglePathAttribute>());

            var statusInfoWatcher = new StatusInfoWatcher(attributeManipulatorMock.Object);
            statusInfoWatcher.Watch(statusInfo);

            // Act
            Action act = () => { statusInfo.StatusType = newStatusType; };

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("The attribute defining the image path was not found for status type: -2.");

            attributeManipulatorMock.Verify(am => am.GetAttributes<StatusTypes, SinglePathAttribute>(It.IsAny<StatusTypes>()),
                Times.Once());
            attributeManipulatorMock.Verify(am => am.GetAttributes<StatusTypes, StatusTextInfoAttribute>(It.IsAny<StatusTypes>()),
                Times.Never());
        }

        [Fact]
        public void Watch_WithNoStatusTextInfoAttribute_ThrowInvalidOperationException()
        {
            // Arrange
            var newStatusType = (StatusTypes)(-2);
            var statusInfo = new StatusInfo { StatusType = (StatusTypes)(-1) };

            var imagePath = @"C:\Test\Image\Path\foo.test";
            attributeManipulatorMock.Setup(am => am.GetAttributes<StatusTypes, SinglePathAttribute>(newStatusType))
                .Returns(new[] { new SinglePathAttribute(imagePath) });

            attributeManipulatorMock.Setup(am => am.GetAttributes<StatusTypes, StatusTextInfoAttribute>(newStatusType))
                .Returns(Enumerable.Empty<StatusTextInfoAttribute>());

            var statusInfoWatcher = new StatusInfoWatcher(attributeManipulatorMock.Object);
            statusInfoWatcher.Watch(statusInfo);

            // Act
            Action act = () => { statusInfo.StatusType = newStatusType; };

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("The attribute defining the status info text was not found for status type: -2.");

            attributeManipulatorMock.Verify(am => am.GetAttributes<StatusTypes, SinglePathAttribute>(It.IsAny<StatusTypes>()),
                Times.Once());
            attributeManipulatorMock.Verify(am => am.GetAttributes<StatusTypes, StatusTextInfoAttribute>(It.IsAny<StatusTypes>()),
                Times.Once());
        }
    }
}
