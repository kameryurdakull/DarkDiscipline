using System;

namespace DarkDiscipline.Application.Events
{
    public interface IEventBus
    {
        EventSubscription Subscribe<TEvent>(Action<TEvent> handler);
        void Publish<TEvent>(TEvent payload);
    }
}
