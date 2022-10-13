using System;
using System.Collections;

namespace MakoIoT.Device.Services.Mediator
{
    public class MediatorOptions
    {
        internal readonly ArrayList Subscribers = new();

        /// <summary>
        /// Adds subscriber (event handler) to an event.
        /// </summary>
        /// <param name="eventType">Type of the event. The event must implement IEvent interface.</param>
        /// <param name="subscriberType">Type of the subscriber (as registered in DI). The subscriber must implement IEventHandler interface.</param>

        public void AddSubscriber(Type eventType, Type subscriberType)
        {
            Subscribers.Add(new Subscription(eventType, subscriberType));
        }
    }

    internal class Subscription
    {
        internal Type EventType { get; }
        internal Type SubscriberType { get; }

        internal Subscription(Type eventType, Type subscriberType)
        {
            EventType = eventType;
            SubscriberType = subscriberType;
        }
    }
}
