using System;
using System.Windows.Input;

namespace SubtitleFileCleanerGUI.Application.Abstractions.Service.Input
{
    public interface ICommandCreator
    {
        public ICommand Create(Action execute, Func<bool> canExecute = null);
        public ICommand Create<TParameter>(Action<TParameter> execute, Predicate<TParameter> canExecute = null);
    }
}
