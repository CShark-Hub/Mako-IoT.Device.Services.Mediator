using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;
using System;

namespace MakoIoT.Device.Services.Mediator.Test
{
    [TestClass]
    public class MediatorTest
    {
        [TestMethod]
        public void Publish_should_invoke_handler()
        {
            var serviceCollection = new ServiceCollection();
            var handler = new TestEventHandler();
            serviceCollection.AddSingleton(typeof(TestEventHandler), handler);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var sut = new Mediator(null, serviceProvider);
            sut.Subscribe(typeof(TestEvent), typeof(TestEventHandler));

            var @event = new TestEvent();
            sut.Publish(@event);

            Assert.AreSame(@event, handler.Event);
        }

        [TestMethod]
        public void Unsubscribe_should_remove_handler()
        {
            var serviceCollection = new ServiceCollection();
            var handler = new TestEventHandler();
            serviceCollection.AddSingleton(typeof(TestEventHandler), handler);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var sut = new Mediator(null, serviceProvider);
            sut.Subscribe(typeof(TestEvent), typeof(TestEventHandler));

            sut.Unsubscribe(typeof(TestEvent), typeof(TestEventHandler));

            var @event = new TestEvent();
            sut.Publish(@event);

            Assert.IsNull(handler.Event);
        }

        [TestMethod]
        public void Publish_should_invoke_handlers_based_on_event_type()
        {
            var serviceCollection = new ServiceCollection();
            var handler = new TestEventHandler();
            serviceCollection.AddSingleton(typeof(TestEventHandler), handler);

            var handler2 = new TestEventHandler2();
            serviceCollection.AddSingleton(typeof(TestEventHandler2), handler2);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var sut = new Mediator(null, serviceProvider);
            sut.Subscribe(typeof(TestEvent), typeof(TestEventHandler));
            sut.Subscribe(typeof(TestEvent2), typeof(TestEventHandler2));

            var @event = new TestEvent();
            var @event2 = new TestEvent2();
            sut.Publish(@event);
            sut.Publish(@event2);

            Assert.AreSame(@event, handler.Event);
            Assert.AreSame(@event2, handler2.Event);
        }

        [TestMethod]
        public void Publish_should_invoke_multiple_handlers_on_event()
        {
            var serviceCollection = new ServiceCollection();
            var handler = new TestEventHandler();
            serviceCollection.AddSingleton(typeof(TestEventHandler), handler);

            var handler2 = new TestEventHandler2();
            serviceCollection.AddSingleton(typeof(TestEventHandler2), handler2);
            var serviceProvider = serviceCollection.BuildServiceProvider();


            var sut = new Mediator(null, serviceProvider);
            sut.Subscribe(typeof(TestEvent), typeof(TestEventHandler));
            sut.Subscribe(typeof(TestEvent), typeof(TestEventHandler2));

            var @event = new TestEvent();
            sut.Publish(@event);

            Assert.AreSame(@event, handler.Event);
            Assert.AreSame(@event, handler2.Event);
        }
    }
}
