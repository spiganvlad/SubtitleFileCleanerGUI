﻿namespace SubtitleFileCleanerGUI.Service.Dialog
{
    public interface IDialogOpener
    {
        public bool? ShowFileDialog(out string filePath);
        public bool? ShowFolderDialog(out string folderPath);
    }
}
