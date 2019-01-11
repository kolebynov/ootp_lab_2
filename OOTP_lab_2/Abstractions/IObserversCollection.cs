using System;

namespace OOTP_lab_2.Abstractions
{
    public interface IObserversCollection<T>
    {
        IDisposable Add(IObserver<T> observer);

        void OnNext(T value);

        void OnError(Exception error);
    }
}
