using SubtitleBytesClearFormatting.Cleaner;

namespace SubtitleFileCleanerGUI.Model
{
    // Supported subtitle cleaners
    public enum SubtitleCleaners
    {
        Auto,
        [SubtitleCleaner(typeof(SrtCleaner))]
        [SubtitleTags("GetBasicTags")]
        [SubtitleExtension(".srt")]
        Srt,
        [SubtitleCleaner(typeof(AssCleaner))]
        [SubtitleTags("GetAssSpecificTags")]
        [SubtitleExtension(".ass")]
        Ass,
        [SubtitleCleaner(typeof(VttCleaner))]
        [SubtitleTags("GetBasicTags")]
        [SubtitleExtension(".vtt")]
        Vtt,
        [SubtitleCleaner(typeof(SbvCleaner))]
        [SubtitleTags("GetBasicTags")]
        [SubtitleExtension(".sbv")]
        Sbv,
        [SubtitleCleaner(typeof(SubCleaner))]
        [SubtitleTags("GetSubSpecificTags")]
        [SubtitleExtension(".sub")]
        Sub
    }

    // Interface describes an object that stores information about the cleaner type
    public interface ICleanable
    {
        public SubtitleCleaners Cleaner { get; set; }
    }
}
