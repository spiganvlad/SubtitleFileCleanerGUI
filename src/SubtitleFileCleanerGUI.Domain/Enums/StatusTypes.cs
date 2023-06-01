using SubtitleFileCleanerGUI.Domain.Attributes;

namespace SubtitleFileCleanerGUI.Domain.Enums
{
    // Supported status types
    public enum StatusTypes
    {
        [SinglePath("/Images/WaitingProcess.png")]
        [StatusTextInfo("Waiting for a process to start")]
        WaitingProcess,
        [SinglePath("/Images/ConvertingProcess.png")]
        [StatusTextInfo("The conversion has begun")]
        ConvertingProcess,
        [SinglePath("/Images/CompletedProcess.png")]
        [StatusTextInfo("The conversion was successful")]
        CompletedProcess,
        [SinglePath("/Images/FailedProcess.png")]
        [StatusTextInfo("An error has occurred")]
        FailedProcess
    }
}
