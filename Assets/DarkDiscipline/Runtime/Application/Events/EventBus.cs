using System;
using System.Collections.Generic;
using System.Linq;

namespace DarkDiscipline.Application.Events
{
    public sealed class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _handlersByType = new();

        public EventSubscription Subscribe<TEvent>(Action<TEvent> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var eventType = typeof(TEvent);

            if (_handlersByType.TryGetValue(eventType, out var handlers) == false)
            {
                handlers = new List<Delegate>();
                _handlersByType.Add(eventType, handlers);
            }

            handlers.Add(handler);
            return new EventSubscription(() => handlers.Remove(handler));
        }

        public void Publish<TEvent>(TEvent payload)
        {
            var eventType = typeof(TEvent);

            if (_handlersByType.TryGetValue(eventType, out var handlers) == false)
            {
                return;
            }

            foreach (var handler in handlers.OfType<Action<TEvent>>().ToArray())
            {
                handler.Invoke(payload);
            }
        }
    }
}
