namespace SubtitleFileCleanerGUI.Model
{
    public interface IUpdatebleSettings
    {
        public SettingsTypes SettingsType { get; set; }
        public void UpdateSettings(ICustomSettings newSettings, SettingsTypes targetType);
    }
}
