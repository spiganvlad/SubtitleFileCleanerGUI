using SubtitleFileCleanerGUI.Application.UnitTests.Helpers.TestObjects.Attributes;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Helpers.TestObjects.Enums
{
    public enum TestAttributedEnum
    {
        [FirstTest]
        WithFirstTestAttribute,
        [FirstTest]
        [FirstTest]
        WithTwoFirstTestAttributes,
        [FirstTest]
        [SecondTest]
        WithFirstAndSecondTestAttributes,
        WithZeroAttributes
    }
}
