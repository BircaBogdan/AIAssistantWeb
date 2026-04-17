using System.Collections.Generic;

namespace AIAssistant.Core.Observers
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }
}