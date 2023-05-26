using System;
using System.Windows.Input;

namespace SubtitleFileCleanerGUI.Service.Input
{
    public interface IParameterizedCommandCreator
    {
        public ICommand Create<TParameter>(Action<TParameter> execute, Predicate<TParameter> canExecute = null);
    }
}
