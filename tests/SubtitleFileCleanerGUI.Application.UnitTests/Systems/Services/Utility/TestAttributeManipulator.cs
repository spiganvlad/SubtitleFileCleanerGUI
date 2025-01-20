using FluentAssertions;
using SubtitleFileCleanerGUI.Application.Service.Utility;
using SubtitleFileCleanerGUI.Application.UnitTests.Helpers.TestObjects.Attributes;
using SubtitleFileCleanerGUI.Application.UnitTests.Helpers.TestObjects.Enums;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.Utility
{
    public class TestAttributeManipulator
    {
        private readonly AttributeManipulator attributeManipulator;

        public TestAttributeManipulator()
        {
            attributeManipulator = new AttributeManipulator();
        }

        [Fact]
        public void GetAttributes_WithOneFirstTestAttribute_ReturnIEnumerableWithOneElement()
        {
            // Arrange
            var enumValue = TestAttributedEnum.WithFirstTestAttribute;

            // Act
            var result = attributeManipulator.GetAttributes<TestAttributedEnum, FirstTestAttribute>(enumValue);

            // Assert
            result.Should().NotBeNull()
                .And.ContainSingle();
        }

        [Fact]
        public void GetAttribute_WithTwoFirstTestAttributes_ReturnIEnumerableWithTwoElements()
        {
            // Arrange
            var enumValue = TestAttributedEnum.WithTwoFirstTestAttributes;

            // Act
            var result = attributeManipulator.GetAttributes<TestAttributedEnum, FirstTestAttribute>(enumValue);

            // Assert
            result.Should().NotBeNull()
                .And.HaveCount(2);
        }

        [Fact]
        public void GetAttribute_WithOneFirstAndOneSecondTestAttributes_ReturnIEnumerableWithOneElement()
        {
            // Arrange
            var enumValue = TestAttributedEnum.WithFirstAndSecondTestAttributes;

            // Act
            var result = attributeManipulator.GetAttributes<TestAttributedEnum, FirstTestAttribute>(enumValue);

            // Assert
            result.Should().NotBeNull()
                .And.ContainSingle();

        }

        [Fact]
        public void GetAttribute_WithZeroAttributes_ReturnEmptyIEnumerable()
        {
            // Arrange
            var enumValue = TestAttributedEnum.WithZeroAttributes;

            // Act
            var result = attributeManipulator.GetAttributes<TestAttributedEnum, FirstTestAttribute>(enumValue);

            // Assert
            result.Should().NotBeNull()
                .And.BeEmpty();
        }
    }
}
