using System;
using System.Windows.Input;

namespace SubtitleFileCleanerGUI.Service.Input
{
    public class CommandCreator : ICommandCreator
    {
        private readonly IParameterlessCommandCreator parameterlessCommandCreator;
        private readonly IParameterizedCommandCreator parameterizedCommandCreator;

        public CommandCreator(IParameterlessCommandCreator parameterlessCommandCreator,
            IParameterizedCommandCreator parameterizedCommandCreator)
        {
            this.parameterlessCommandCreator = parameterlessCommandCreator;
            this.parameterizedCommandCreator = parameterizedCommandCreator;
        }

        public ICommand Create(Action execute, Func<bool> canExecute = null)
        {
            return parameterlessCommandCreator.Create(execute, canExecute);
        }

        public ICommand Create<TParameter>(Action<TParameter> execute, Predicate<TParameter> canExecute = null)
        {
            return parameterizedCommandCreator.Create(execute, canExecute);
        }
    }
}
