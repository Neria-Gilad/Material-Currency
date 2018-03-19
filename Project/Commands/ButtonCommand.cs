using System;
using System.Windows.Input;

namespace Project.Commands {
    class ButtonCommand : ICommand {//anti-coupling command. just give it an action to perform

        private Action _what { get; set; }
        private Func<bool> _when { get; set; }

        public ButtonCommand(Action what, Func<bool> when)
        {
            _what = what;
            _when = when;
        }

        public bool CanExecute(object parameter)
        {
            return _when();
        }

        public void Execute(object parameter)
        {
            _what();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
