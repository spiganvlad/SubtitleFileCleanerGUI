namespace SubtitleFileCleanerGUI.Model
{
    // Interface describes the status which stores its description and paths to the displayed image
    public interface IViewableStatus
    {
        public string ImagePath { get; set; }
        public string TextInfo { get; set; }
    }
}
