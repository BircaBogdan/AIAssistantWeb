using System.Collections.Generic;

namespace AIAssistant.Core.Observers
{
    public class MessageLimitSubject : ISubject
    {
        private readonly List<IObserver> _observers = new();
        private int _messageCount;

        public void SetMessageCount(int count)
        {
            _messageCount = count;
            Notify();
        }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_messageCount);
            }
        }
    }
}