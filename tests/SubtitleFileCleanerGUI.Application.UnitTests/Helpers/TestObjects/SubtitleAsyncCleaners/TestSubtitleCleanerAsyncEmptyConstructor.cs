using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SubtitleBytesClearFormatting.Cleaners;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Helpers.TestObjects.SubtitleAsyncCleaners
{
    public class TestSubtitleCleanerAsyncEmptyConstructor : ISubtitleCleanerAsync
    {
        public TestSubtitleCleanerAsyncEmptyConstructor() { }

        public Task<List<byte>> DeleteFormattingAsync(byte[] subtitleBytes)
        {
            throw new NotImplementedException();
        }
    }
}
