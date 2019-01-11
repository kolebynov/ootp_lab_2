using System;
using System.Collections.Generic;
using OOTP_lab_2.Abstractions;

namespace OOTP_lab_2.Implementations
{
    public class ObserversCollection<T> : IObserversCollection<T>
    {
        private readonly List<IObserver<T>> _observers = new List<IObserver<T>>();

        public IDisposable Add(IObserver<T> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }
            
            _observers.Add(observer);
            return new ObserverDispose(_observers, observer);
        }

        public void OnNext(T value)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(value);
            }
        }

        public void OnError(Exception error)
        {
            foreach (var observer in _observers)
            {
                observer.OnError(error);
            }
        }

        private class ObserverDispose : IDisposable
        {
            private readonly List<IObserver<T>> _observers;
            private readonly IObserver<T> _observer;

            public ObserverDispose(List<IObserver<T>> observers, IObserver<T> observer)
            {
                if (observers == null)
                {
                    throw new ArgumentNullException(nameof(observers));
                }

                if (observer == null)
                {
                    throw new ArgumentNullException(nameof(observer));
                }

                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                _observers.Remove(_observer);
            }
        }
    }
}
