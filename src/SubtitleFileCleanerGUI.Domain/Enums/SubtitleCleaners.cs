using SubtitleBytesClearFormatting.Cleaners;
using SubtitleBytesClearFormatting.TagsGenerate;
using SubtitleFileCleanerGUI.Domain.Attributes;

namespace SubtitleFileCleanerGUI.Domain.Enums
{
    // Supported subtitle cleaners
    public enum SubtitleCleaners
    {
        Auto,
        [SubtitleCleanerAsyncType(typeof(SrtCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetBasicTags))]
        [SubtitleExtension(".srt")]
        Srt,
        [SubtitleCleanerAsyncType(typeof(AssCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetAssSpecificTags))]
        [SubtitleExtension(".ass")]
        Ass,
        [SubtitleCleanerAsyncType(typeof(VttCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetBasicTags))]
        [SubtitleExtension(".vtt")]
        Vtt,
        [SubtitleCleanerAsyncType(typeof(SbvCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetBasicTags))]
        [SubtitleExtension(".sbv")]
        Sbv,
        [SubtitleCleanerAsyncType(typeof(SubCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetSubSpecificTags))]
        [SubtitleExtension(".sub")]
        Sub
    }
}
