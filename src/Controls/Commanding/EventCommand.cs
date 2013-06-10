using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Controls.Commanding
{
    public class EventCommand : ICommand
    {
        private readonly object _sender;

        public EventCommand() {}

        public EventCommand(object sender) { _sender = sender; }

        private event EventHandler<EventCommandEventArgs> Event;

        public virtual void AddHandler(EventHandler<EventCommandEventArgs> handler)
        {
            Event += handler;
            OnCanExecuteChanged();
        }

        public virtual void RemoveHandler(EventHandler<EventCommandEventArgs> handler)
        {
            Event -= handler;
            OnCanExecuteChanged();
        }

        public void Execute(object parameter)
        {
            if (Event != null) Event(_sender ?? this, new EventCommandEventArgs(parameter));
        }

        public bool CanExecute(object parameter)
        {
            return Event != null;
        }

        protected virtual void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null) CanExecuteChanged(this, null);
        }

        public event EventHandler CanExecuteChanged;

        public static EventCommand operator +(EventCommand command, EventHandler<EventCommandEventArgs> handler)
        {
            command.AddHandler(handler);
            return command;
        }

        public static EventCommand operator -(EventCommand command, EventHandler<EventCommandEventArgs> handler)
        {
            command.RemoveHandler(handler);
            return command;
        }
    }

    public class EventCommandEventArgs : EventArgs
    {
        public object Parameter { get; private set; }

        public EventCommandEventArgs(object parameter)
        {
            Parameter = parameter;
        }
    }
}
