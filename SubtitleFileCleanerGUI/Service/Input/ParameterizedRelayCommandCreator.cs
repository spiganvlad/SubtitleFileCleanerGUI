using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SubtitleFileCleanerGUI.Service.Input
{
    public class ParameterizedRelayCommandCreator : IParameterizedCommandCreator
    {
        public ICommand Create<TParameter>(Action<TParameter> execute, Predicate<TParameter> canExecute = null)
        {
            if (canExecute == null)
                return new RelayCommand<TParameter>(execute);

            return new RelayCommand<TParameter>(execute, canExecute);
        }
    }
}
