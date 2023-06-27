using SubtitleBytesClearFormatting.Cleaners;
using SubtitleBytesClearFormatting.TagsGenerate;
using SubtitleFileCleanerGUI.Domain.Attributes;

namespace SubtitleFileCleanerGUI.Domain.Enums
{
    // Supported subtitle cleaners
    public enum SubtitleCleaners
    {
        Auto,
        [SubtitleCleaner(typeof(SrtCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetBasicTags))]
        [SubtitleExtension(".srt")]
        Srt,
        [SubtitleCleaner(typeof(AssCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetAssSpecificTags))]
        [SubtitleExtension(".ass")]
        Ass,
        [SubtitleCleaner(typeof(VttCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetBasicTags))]
        [SubtitleExtension(".vtt")]
        Vtt,
        [SubtitleCleaner(typeof(SbvCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetBasicTags))]
        [SubtitleExtension(".sbv")]
        Sbv,
        [SubtitleCleaner(typeof(SubCleaner))]
        [SubtitleTags(nameof(TagsCollectionGeneretor.GetSubSpecificTags))]
        [SubtitleExtension(".sub")]
        Sub
    }
}
