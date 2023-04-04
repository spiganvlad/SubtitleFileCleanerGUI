using System;
using System.Windows.Input;

namespace SubtitleFileCleanerGUI.Service.Input
{
    public interface ICommandCreator
    {
        public ICommand Create(Action execute, Func<bool> canExecute = null);
    }
}
