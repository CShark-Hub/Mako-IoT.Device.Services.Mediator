using nanoFramework.DependencyInjection;
using System;
using System.Collections;

namespace MakoIoT.Device.Services.Mediator
{
    /// <summary>
    /// Simple implementation of Mediator pattern
    /// </summary>
    public class Mediator : IMediator
    {
        private readonly Hashtable _subscribers;
        private readonly IServiceProvider serviceProvider;

        public Mediator(MediatorOptions options, IServiceProvider serviceProvider)
        {
            _subscribers = new();
            if (options != null)
            {
                foreach (Subscription sub in options.Subscribers)
                {
                    Subscribe(sub.EventType, sub.SubscriberType);
                }
            }

            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Subscribes event handler to an event.
        /// </summary>
        /// <param name="eventType">Type of the event. The event must implement IEvent interface.</param>
        /// <param name="subscriberType">Type of the subscriber (as registered in DI). The subscriber must implement IEventHandler interface.</param>
        public void Subscribe(Type eventType, Type subscriberType)
        {
            string eventName = eventType.FullName;
            if (!_subscribers.Contains(eventName))
            {
                _subscribers.Add(eventName, new ArrayList { subscriberType });
                return;
            }

            var subscribers = (ArrayList)_subscribers[eventName];
            if (!subscribers.Contains(subscriberType))
                subscribers.Add(subscriberType);
        }

        /// <summary>
        /// Unsubscribes event handler from an event.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="subscriberType">Type of the subscriber (as registered in DI).</param>
        public void Unsubscribe(Type eventType, Type subscriberType)
        {
            string eventName = eventType.FullName;
            if (!_subscribers.Contains(eventName))
                return;

            var subscribers = (ArrayList)_subscribers[eventName];
            if (subscribers.Contains(subscriberType))
                subscribers.Remove(subscriberType);
        }

        /// <summary>
        /// Publishes on event and calls each subscriber.
        /// </summary>
        /// <param name="e">The event.</param>
        public void Publish(IEvent e)
        {
            string eventName = e.GetType().FullName;
            if (_subscribers.Contains(eventName))
            {
                foreach (Type subscriberType in (ArrayList)_subscribers[eventName])
                {
                    ((IEventHandler)serviceProvider.GetService(subscriberType)).Handle(e);
                }
            }
        }
    }
}
