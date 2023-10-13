using System;
using System.Collections;

namespace MakoIoT.Device.Services.Mediator
{
    /// <summary>
    /// Simple implementation of Mediator pattern
    /// </summary>
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Hashtable _subscribers = new();
        private readonly Hashtable _subscriberTypes = new();

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Mediator(MediatorOptions options, IServiceProvider serviceProvider): this(serviceProvider)
        {
            if (options is not null)
            {
                foreach (MediatorOptionsSubscriber subscriber in options.Subscribers)
                {
                    Subscribe(subscriber.EventType, subscriber.SubscriberType);
                }
            }
        }

        /// <inheritdoc />
        public void Subscribe(Type eventType, IEventHandler subscriber)
        {
            var eventName = eventType.FullName;
            if (!_subscribers.Contains(eventName))
            {
                _subscribers.Add(eventName, new ArrayList { subscriber });
                return;
            }

            var subscribers = (ArrayList) _subscribers[eventName];
            if (!subscribers.Contains(subscriber))
            {
                subscribers.Add(subscriber);
            }
        }

        /// <inheritdoc />
        public void Subscribe(Type eventType, Type subscriberType)
        {
            var eventName = eventType.FullName;
            if (!_subscriberTypes.Contains(eventName))
            {
                _subscriberTypes.Add(eventName, new ArrayList { subscriberType });
                return;
            }

            var subscribers = (ArrayList) _subscriberTypes[eventName];
            if (!subscribers.Contains(subscriberType))
            {
                subscribers.Add(subscriberType);
            }
        }

        /// <inheritdoc />
        public void Unsubscribe(Type eventType, IEventHandler subscriber)
        {
            var eventName = eventType.FullName;
            if (!_subscribers.Contains(eventName))
            {
                return;
            }

            var subscribers = (ArrayList) _subscribers[eventName];
            if (subscribers.Contains(subscriber))
            {
                subscribers.Remove(subscriber);
            }
        }

        /// <inheritdoc />
        public void Unsubscribe(Type eventType, Type subscriberType)
        {
            var eventName = eventType.FullName;
            if (!_subscriberTypes.Contains(eventName))
            {
                return;
            }

            var subscribers = (ArrayList) _subscriberTypes[eventName];
            if (subscribers.Contains(subscriberType))
            {
                subscribers.Remove(subscriberType);
            }
        }

        /// <inheritdoc />
        public void Publish(IEvent e)
        {
            var eventName = e.GetType().FullName;

            if (_subscribers.Contains(eventName))
            {
                foreach (IEventHandler subscriber in (ArrayList) _subscribers[eventName])
                {
                    subscriber.Handle(e);
                }
            }

            if (_subscriberTypes.Contains(eventName))
            {
                foreach (Type subscriberType in (ArrayList) _subscriberTypes[eventName])
                {
                    ((IEventHandler)_serviceProvider.GetService(subscriberType)).Handle(e);
                }
            }
        }
    }
}
