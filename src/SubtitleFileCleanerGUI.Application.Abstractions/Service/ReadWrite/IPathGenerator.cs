namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.ReadWrite
{
    public interface IPathGenerator
    {
        public string CreateUniquePath(string path);
    }
}
