using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TourPlanner.UI.ViewModels
{
    /// <summary>
    /// The class for the RelayCommand. 
    /// 
    /// This implementation makes it possible to map Commands to UI-Components (e.g. button-presses).
    /// 
    /// </summary>
    public class RelayCommand : ICommand
    {
        // The method that gets executed when the RelayCommand geht triggered.
        private Action<object> execute;

        // Manages if the Command can be executed.
        private Func<object,bool> canExecute;


        public event EventHandler? CanExecuteChanged 
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// The constructor for the relay command.
        /// </summary>
        /// <param name="execute">The method that should be triggered as Action<T></param>
        /// <param name="canExecute">A boolean that determines wheter or not the method can be executed. </param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Method that gets calles dynamically to check if the Command can be executed.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        /// <summary>
        /// Will be triggered if the user presses a button for example.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }
}
