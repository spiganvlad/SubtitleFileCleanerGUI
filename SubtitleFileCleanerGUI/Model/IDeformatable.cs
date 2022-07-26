namespace SubtitleFileCleanerGUI.Model
{
    // Interface describes an object that stores information about the need to remove tags
    public interface IDeformatable
    {
        public bool DeleteTags { get; set; }
    }
}
