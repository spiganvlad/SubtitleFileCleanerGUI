using System;
using System.Windows.Input;

namespace SubtitleFileCleanerGUI.Service.Input
{
    public interface IGenericCommandCreator
    {
        public ICommand Create<TParameter>(Action<TParameter> execute, Predicate<TParameter> canExecute = null);
    }
}
