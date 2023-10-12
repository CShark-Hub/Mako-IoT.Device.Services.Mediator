#  Mako-IoT.Device.Services.Mediator
Mediator pattern implementation. Provides in-process publisher-subscriber communication while keeping all parties decoupled.

## Usage
See [Mediator](https://github.com/CShark-Hub/Mako-IoT.Device.Samples/tree/main/Mediator) sample

Create classes for your events
```c#
public class Event1 : IEvent
{
    public string Data { get; set; }
}

public class Event2 : IEvent
{
    public string Text { get; set; }
}
```
Your event subscriber must implement _IEventHandler_ interface
```c#
public class Service2 : IEventHandler
{
    public void Handle(IEvent @event)
    {
        switch (@event)
        {
            case Event1 event1:
                Debug.WriteLine($"[{nameof(Service2)}] Event1 received. The data is: {event1.Data}");
                break;
            case Event2 event2:
                Debug.WriteLine($"[{nameof(Service2)}] Event2 received The text is: {event2.Text}");
                break;
        }
    }
}
```
Use _IMediator_ to publish events
```c#
public class Service1 : IService1
{
    private readonly IMediator _mediator;

    public Service1(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void DoSomething()
    {
        _mediator.Publish(new Event2 { Text = "Hello from Service1 !" });
    }
}
```
Register Mediator and singleton subscribers in [_DeviceBuilder_](https://github.com/CShark-Hub/Mako-IoT.Device)
```c#
DeviceBuilder.Create()
  .AddMediator(options =>
  {
      options.AddSubscriber(typeof(Event1), typeof(Service2));
      options.AddSubscriber(typeof(Event2), typeof(Service2));
  })
  .Build()
  .Start()
```
For transient and scoped services you can use the `Subscribe` and `Unsubscribe` overloads that take a specific instance.
```c#
public class TransientService : IDisposable
{
    private readonly IMediator _mediator;

    public TransientService(IMediator mediator)
    {
        _mediator = mediator;
        _mediator.Subscribe(typeof(Event1), this);
        _mediator.Subscribe(typeof(Event2), this);
    }

    public void Dispose()
    {
        _mediator.Unsubscribe(typeof(Event1), this);
        _mediator.Unsubscribe(typeof(Event2), this);
    }
}
```
