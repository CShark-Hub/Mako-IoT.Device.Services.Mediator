using System;

namespace MakoIoT.Device.Services.Mediator
{
    public interface IMediator
    {
        /// <summary>
        /// Subscribes event handler to an event.
        /// </summary>
        /// <param name="eventType">Type of the event. The event must implement IEvent interface.</param>
        /// <param name="subscriber">The subscriber.</param>
        void Subscribe(Type eventType, IEventHandler subscriber);

        /// <summary>
        /// Subscribes event handler to an event.
        /// </summary>
        /// <param name="eventType">Type of the event. The event must implement IEvent interface.</param>
        /// <param name="subscriberType">Type of the subscriber (as registered in DI). The subscriber must implement IEventHandler interface.</param>
        void Subscribe(Type eventType, Type subscriberType);

        /// <summary>
        /// Unsubscribes event handler from an event.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="subscriber">The subscriber.</param>
        public void Unsubscribe(Type eventType, IEventHandler subscriber);

        /// <summary>
        /// Unsubscribes event handler from an event.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="subscriberType">Type of the subscriber (as registered in DI).</param>
        void Unsubscribe(Type eventType, Type subscriberType);

        /// <summary>
        /// Publishes on event and calls each subscriber.
        /// </summary>
        /// <param name="e">The event.</param>
        void Publish(IEvent e);
    }
}