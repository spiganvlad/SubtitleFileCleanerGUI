using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using SubtitleFileCleanerGUI.Application.Abstractions.Enums;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Utility;
using SubtitleFileCleanerGUI.Application.Service.Settings;
using SubtitleFileCleanerGUI.Application.UnitTests.Helpers.Builders;
using SubtitleFileCleanerGUI.Domain.Attributes;
using SubtitleFileCleanerGUI.Domain.Enums;
using SubtitleFileCleanerGUI.Domain.Model;
using Xunit;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Systems.Services.Settings
{
    public class TestDefaultFileManipulator
    {
        private readonly Mock<IAttributeManipulator> attributeManipulatorMock;

        public TestDefaultFileManipulator()
        {
            attributeManipulatorMock = new Mock<IAttributeManipulator>();
        }

        [Fact]
        public void GetDefaultFile_WithValidConfiguration_ReturnValidSubtitleFile()
        {
            // Arrange
            var defaultFileType = DefaultFileTypes.Default;

            var path = "testFolder/testFile.test";
            var cleaner = SubtitleCleaners.Auto;
            var deleteTags = true;
            var toOneLine = true;

            var sectionPath = "testRoot:testSubRoot:testSectionPath";
            var pathSectionPath = $"{sectionPath}:PathDestination";
            var cleanerSectionPath = $"{sectionPath}:SubtitleConverter";
            var deleteTagsSectionPath = $"{sectionPath}:DeleteTagsOption";
            var toOneLineSectionPath = $"{sectionPath}:ToOneLineOption";

            attributeManipulatorMock.Setup(am => am.GetAttributes<DefaultFileTypes, SinglePathAttribute>(defaultFileType))
                .Returns(new List<SinglePathAttribute> { new(sectionPath) });

            var configurationMockBuilder = new ConfigurationMockBuilder();
            var configurationMock = configurationMockBuilder
                .SetupSection(pathSectionPath, path, out Mock<IConfigurationSection> pathSection)
                .SetupSection(cleanerSectionPath, cleaner.ToString(), out Mock<IConfigurationSection> cleanerSection)
                .SetupSection(deleteTagsSectionPath, deleteTags.ToString(), out Mock<IConfigurationSection> deleteTagsSection)
                .SetupSection(toOneLineSectionPath, toOneLine.ToString(), out Mock<IConfigurationSection> toOneLineSection)
                .Build();

            var defaultFileManipulator = new DefaultFileManipulator(configurationMock.Object, attributeManipulatorMock.Object);

            // Act
            var result = defaultFileManipulator.GetDefaultFile(defaultFileType);

            // Assert
            attributeManipulatorMock.Verify(am => am.GetAttributes<DefaultFileTypes, SinglePathAttribute>(It.IsAny<DefaultFileTypes>()),
                Times.Once());

            pathSection.VerifyGet(s => s.Value, Times.Once());
            cleanerSection.VerifyGet(s => s.Value, Times.Once());
            deleteTagsSection.VerifyGet(s => s.Value, Times.Once());
            toOneLineSection.VerifyGet(s => s.Value, Times.Once());

            configurationMock.Verify(c => c.GetSection(pathSectionPath), Times.Once());
            configurationMock.Verify(c => c.GetSection(cleanerSectionPath), Times.Once());
            configurationMock.Verify(c => c.GetSection(deleteTagsSectionPath), Times.Once());
            configurationMock.Verify(c => c.GetSection(toOneLineSectionPath), Times.Once());

            result.Should().NotBeNull();
            result.PathDestination.Should().NotBeNull().And.Be(path);
            result.Cleaner.Should().Be(cleaner);
            result.DeleteTags.Should().Be(deleteTags);
            result.ToOneLine.Should().Be(toOneLine);
        }

        [Fact]
        public void SetDefaultFile_WithValidConfiguration_WorkValid()
        {
            // Arrange
            var defaultFileType = DefaultFileTypes.Default;

            var path = "testFolder/testFile.test";
            var cleaner = SubtitleCleaners.Auto;
            var deleteTags = true;
            var toOneLine = true;

            var subtitleFile = new SubtitleFile
            {
                PathDestination = path,
                Cleaner = cleaner,
                DeleteTags = deleteTags,
                ToOneLine = toOneLine
            };

            var sectionPath = "testRoot:testSubRoot:testSectionPath";
            var pathSectionPath = $"{sectionPath}:PathDestination";
            var cleanerSectionPath = $"{sectionPath}:SubtitleConverter";
            var deleteTagsSectionPath = $"{sectionPath}:DeleteTagsOption";
            var toOneLineSectionPath = $"{sectionPath}:ToOneLineOption";

            attributeManipulatorMock.Setup(am => am.GetAttributes<DefaultFileTypes, SinglePathAttribute>(defaultFileType))
                .Returns(new List<SinglePathAttribute> { new(sectionPath) });

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupSet(c => c[pathSectionPath] = path);
            configurationMock.SetupSet(c => c[cleanerSectionPath] = cleaner.ToString());
            configurationMock.SetupSet(c => c[deleteTagsSectionPath] = deleteTags.ToString());
            configurationMock.SetupSet(c => c[toOneLineSectionPath] = toOneLine.ToString());

            var defaultFileManipulator = new DefaultFileManipulator(configurationMock.Object, attributeManipulatorMock.Object);

            // Act
            defaultFileManipulator.SetDefaultFile(subtitleFile, defaultFileType);

            // Assert
            attributeManipulatorMock.Verify(am => am.GetAttributes<DefaultFileTypes, SinglePathAttribute>(It.IsAny<DefaultFileTypes>()),
                Times.Once());

            configurationMock.VerifySet(c => c[pathSectionPath] = It.IsAny<string>(), Times.Once());
            configurationMock.VerifySet(c => c[cleanerSectionPath] = It.IsAny<string>(), Times.Once());
            configurationMock.VerifySet(c => c[deleteTagsSectionPath] = It.IsAny<string>(), Times.Once());
            configurationMock.VerifySet(c => c[toOneLineSectionPath] = It.IsAny<string>(), Times.Once());
        }
    }
}
