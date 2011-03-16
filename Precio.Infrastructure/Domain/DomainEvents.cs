using System;
using System.Collections.Generic;
using Precio.Messaging;
using Precio.Transactional;

namespace Precio.Domain
{
    public static class DomainEvents
    {
        [ThreadStatic] private static MessageBus bus ;

        internal static void Init()
        {
            bus = new MessageBus();
        }

        public static void Raise<T>(T @event) where T : class,IDomainEvent 
        {
            bus.Send(@event);
        }

        public static DomainSubscription<T> When<T>() where T:class,IDomainEvent
        {
            return new DomainSubscription<T>();
        }

        internal static void Handle<T>(Action<T> handler) where T : IDomainEvent
        {
            bus.Subscribe(MessageHandlerType.Synchronous,handler,false);
        }
    }

    public class DomainSubscription<T> where T : class,IDomainEvent
    {
        public void Then(Action<T> handler)
        {
            DomainEvents.Handle(handler);
        }

        public DelayedDomainSubscription<T> DelayUntill(Action<Action> waitFor)
        {
            return new DelayedDomainSubscription<T>(waitFor);
        }
    }

    public class DelayedDomainSubscription<T> where T : class,IDomainEvent
    {
        private Action<Action> delayWith;

        public DelayedDomainSubscription(Action<Action> delayWith)
        {
            this.delayWith = delayWith;
        }

        public void Then(Action<T> handler)
        {
            Action<T> wrapped = e => delayWith(() => handler(e));
            DomainEvents.Handle(wrapped);
        }
    }
}