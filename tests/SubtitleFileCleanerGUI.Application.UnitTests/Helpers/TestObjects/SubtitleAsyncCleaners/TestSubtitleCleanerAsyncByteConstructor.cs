using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SubtitleBytesClearFormatting.Cleaners;

namespace SubtitleFileCleanerGUI.Application.UnitTests.Helpers.TestObjects.SubtitleAsyncCleaners
{
    public class TestSubtitleCleanerAsyncByteConstructor : ISubtitleCleanerAsync
    {
        public TestSubtitleCleanerAsyncByteConstructor(byte value) { }

        public Task<List<byte>> DeleteFormattingAsync(byte[] subtitleBytes)
        {
            throw new NotImplementedException();
        }
    }
}
