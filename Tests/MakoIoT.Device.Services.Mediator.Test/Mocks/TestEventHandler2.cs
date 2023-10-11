namespace MakoIoT.Device.Services.Mediator.Test.Mocks
{
    public class TestEventHandler2 : IEventHandler
    {
        public IEvent Event { get; private set; }

        public void Handle(IEvent @event)
        {
            Event = @event;
        }
    }
}
