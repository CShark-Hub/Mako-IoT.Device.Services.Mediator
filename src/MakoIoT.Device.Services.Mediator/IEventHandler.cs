namespace MakoIoT.Device.Services.Mediator
{
    public interface IEventHandler
    {
        void Handle(IEvent @event);
    }
}
