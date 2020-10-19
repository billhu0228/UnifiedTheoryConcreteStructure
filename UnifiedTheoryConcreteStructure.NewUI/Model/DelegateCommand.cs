using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UnifiedTheoryConcreteStructure.NewUI
{
    public class DelegateCommand : ICommand
    {
        private Action action;
        private Action<Object> actionT;

        public DelegateCommand(Action action)
        {
            this.action = action;
        }

        public DelegateCommand(Action<Object> action)
        {
            this.actionT = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (action != null)
            {
                action();
            }
            if (actionT != null)
            {
                actionT.Invoke(parameter);
            }
        }
    }
}
