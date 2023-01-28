using MakoIoT.Device.Services.DependencyInjection;
using nanoFramework.TestFramework;

namespace MakoIoT.Device.Services.Mediator.Test
{
    [TestClass]
    public class MediatorTest
    {
        [TestMethod]
        public void Publish_should_invoke_handler()
        {
            DI.Clear();

            var handler = new TestEventHandler();
            DI.RegisterInstance(typeof(TestEventHandler), handler);

            var sut = new Mediator(null, null);
            sut.Subscribe(typeof(TestEvent), typeof(TestEventHandler));

            var @event = new TestEvent();
            sut.Publish(@event);

            Assert.AreSame(@event, handler.Event);
        }

        [TestMethod]
        public void Unsubscribe_should_remove_handler()
        {
            DI.Clear();

            var handler = new TestEventHandler();
            DI.RegisterInstance(typeof(TestEventHandler), handler);

            var sut = new Mediator(null, null);
            sut.Subscribe(typeof(TestEvent), typeof(TestEventHandler));

            sut.Unsubscribe(typeof(TestEvent), typeof(TestEventHandler));

            var @event = new TestEvent();
            sut.Publish(@event);

            Assert.IsNull(handler.Event);
        }

        [TestMethod]
        public void Publish_should_invoke_handlers_based_on_event_type()
        {
            DI.Clear();

            var handler = new TestEventHandler();
            DI.RegisterInstance(typeof(TestEventHandler), handler);

            var handler2 = new TestEventHandler2();
            DI.RegisterInstance(typeof(TestEventHandler2), handler2);


            var sut = new Mediator(null, null);
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
            DI.Clear();

            var handler = new TestEventHandler();
            DI.RegisterInstance(typeof(TestEventHandler), handler);

            var handler2 = new TestEventHandler2();
            DI.RegisterInstance(typeof(TestEventHandler2), handler2);


            var sut = new Mediator(null, null);
            sut.Subscribe(typeof(TestEvent), typeof(TestEventHandler));
            sut.Subscribe(typeof(TestEvent), typeof(TestEventHandler2));

            var @event = new TestEvent();
            sut.Publish(@event);

            Assert.AreSame(@event, handler.Event);
            Assert.AreSame(@event, handler2.Event);
        }
    }
}
