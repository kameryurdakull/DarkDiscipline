using System;

namespace DarkDiscipline.Application.Events
{
    public sealed class EventSubscription : IDisposable
    {
        private readonly Action _dispose;
        private bool _isDisposed;

        public EventSubscription(Action dispose)
        {
            _dispose = dispose ?? throw new ArgumentNullException(nameof(dispose));
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            _dispose.Invoke();
        }
    }
}
