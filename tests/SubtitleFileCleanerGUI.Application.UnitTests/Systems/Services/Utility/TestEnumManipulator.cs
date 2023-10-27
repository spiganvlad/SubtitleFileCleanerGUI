using FluentAssertions;
using SubtitleFileCleanerGUI.Application.Service.Utility;
using SubtitleFileCleanerGUI.Application.UnitTests.Helpers.TestObjects.Enums;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.Utility
{
    public class TestEnumManipulator
    {
        private readonly EnumManipulator enumManipulator;

        public TestEnumManipulator()
        {
            enumManipulator = new EnumManipulator();
        }

        [Fact]
        public void GetAllEnumValues_WithFiveElementEnum_ReturnIEnumerableWithFiveElements()
        {
            // Arrange
            var expectedEnums = new[]
            {
                TestFiveElementEnum.FirstTestValue,
                TestFiveElementEnum.SecondTestValue,
                TestFiveElementEnum.ThirdTestValue,
                TestFiveElementEnum.FourthTestValue,
                TestFiveElementEnum.FifthTestValue
            };

            // Act
            var result = enumManipulator.GetAllEnumValues<TestFiveElementEnum>();

            // Assert
            result.Should().NotBeNull()
                .And.HaveCount(5)
                .And.ContainInConsecutiveOrder(expectedEnums);
        }

        [Fact]
        public void GetAllEnumValues_WithZeroElementEnum_ReturnEmptyIEnumerable()
        {
            // Act
            var result = enumManipulator.GetAllEnumValues<TestZeroElementEnum>();

            // Assert
            result.Should().NotBeNull()
                .And.BeEmpty();
        }
    }
}
