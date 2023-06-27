using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.Input;

namespace SubtitleFileCleanerGUI.Application.Service.Input
{
    public class ParameterlessRelayCommandCreator : IParameterlessCommandCreator
    {
        public ICommand Create(Action execute, Func<bool> canExecute = null)
        {
            if (canExecute == null)
                return new RelayCommand(execute);

            return new RelayCommand(execute, canExecute);
        }
    }
}
