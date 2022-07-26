namespace SubtitleFileCleanerGUI.Model
{
    // Interface describes an object that stores information about the need to be transformed into one line
    public interface IMinifiable
    {
        public bool ToOneLine { get; set; }
    }
}
