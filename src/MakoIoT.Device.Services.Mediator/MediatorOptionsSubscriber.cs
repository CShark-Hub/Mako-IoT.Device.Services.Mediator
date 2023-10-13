using System;

namespace MakoIoT.Device.Services.Mediator
{
    public class MediatorOptionsSubscriber
    {
        public Type EventType { get; }
        public Type SubscriberType { get; }

        internal MediatorOptionsSubscriber(Type eventType, Type subscriberType)
        {
            EventType = eventType;
            SubscriberType = subscriberType;
        }
    }
}
