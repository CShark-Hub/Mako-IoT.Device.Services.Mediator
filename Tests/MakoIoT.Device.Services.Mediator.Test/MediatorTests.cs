using MakoIoT.Device.Services.Mediator.Test.Mocks;
using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

namespace MakoIoT.Device.Services.Mediator.Test
{
    [TestClass]
    public class MediatorTests
    {
        [TestMethod]
        public void Publish_should_invoke_handler_by_object()
        {
            // Arrange
            var handler = new TestEventHandler();

            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var sut = (IMediator) new Mediator(null, serviceProvider);
            sut.Subscribe(typeof(TestEvent), handler);

            var @event = new TestEvent();

            // Act
            sut.Publish(@event);

            // Assert
            Assert.AreSame(@event, handler.Event);
        }

        [TestMethod]
        public void Publish_should_invoke_handler_by_type()
        {
            // Arrange
            var handler = new TestEventHandler();
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(typeof(TestEventHandler), handler);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var sut = (IMediator) new Mediator(null, serviceProvider);
            sut.Subscribe(typeof(TestEvent), typeof(TestEventHandler));

            var @event = new TestEvent();
            
            // Act
            sut.Publish(@event);

            // Assert
            Assert.AreSame(@event, handler.Event);
        }

        [TestMethod]
        public void Publish_should_invoke_handlers()
        {
            // Arrange
            var handler1 = new TestEventHandler();
            var handler2 = new TestEventHandler();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(typeof(TestEventHandler), handler1);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var sut = (IMediator) new Mediator(null, serviceProvider);
            sut.Subscribe(typeof(TestEvent), typeof(TestEventHandler));
            sut.Subscribe(typeof(TestEvent), handler2);

            var @event = new TestEvent();

            // Act
            sut.Publish(@event);

            // Assert
            Assert.AreSame(@event, handler1.Event);
            Assert.AreSame(@event, handler2.Event);
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

            var sut = (IMediator) new Mediator(null, serviceProvider);
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

            var sut = (IMediator) new Mediator(null, serviceProvider);
            sut.Subscribe(typeof(TestEvent), typeof(TestEventHandler));
            sut.Subscribe(typeof(TestEvent), typeof(TestEventHandler2));

            var @event = new TestEvent();
            sut.Publish(@event);

            Assert.AreSame(@event, handler.Event);
            Assert.AreSame(@event, handler2.Event);
        }

        [TestMethod]
        public void Unsubscribe_should_remove_handler_by_object()
        {
            // Arrange
            var handler1 = new TestEventHandler();
            var handler2 = new TestEventHandler();
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var sut = (IMediator) new Mediator(null, serviceProvider);
            sut.Subscribe(typeof(TestEvent), handler1);
            sut.Subscribe(typeof(TestEvent), handler2);
            sut.Unsubscribe(typeof(TestEvent), handler1);

            var @event = new TestEvent();

            // Act
            sut.Publish(@event);

            // Assert
            Assert.IsNull(handler1.Event);
            Assert.AreSame(@event, handler2.Event);
        }

        [TestMethod]
        public void Unsubscribe_should_remove_handler_by_type()
        {
            // Arrange
            var handler1 = new TestEventHandler();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(typeof(TestEventHandler), handler1);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var sut = (IMediator) new Mediator(null, serviceProvider);
            sut.Subscribe(typeof(TestEvent), typeof(TestEventHandler));
            sut.Unsubscribe(typeof(TestEvent), typeof(TestEventHandler));

            var @event = new TestEvent();

            // Act
            sut.Publish(@event);

            // Assert
            Assert.IsNull(handler1.Event);
        }
    }
}
