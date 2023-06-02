using System;
using System.Windows.Input;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.Input
{
    public interface IParameterlessCommandCreator
    {
        public ICommand Create(Action execute, Func<bool> canExecute = null);
    }
}
