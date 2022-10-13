using MakoIoT.Device.Services.DependencyInjection;
using MakoIoT.Device.Services.Interface;

namespace MakoIoT.Device.Services.Mediator.Extensions
{
    public delegate void MediatorConfigurator(MediatorOptions options);

    public static class DeviceBuilderExtension
    {
        public static IDeviceBuilder AddMediator(this IDeviceBuilder builder)
        {
            return builder.AddMediator(o => { });
        }

        public static IDeviceBuilder AddMediator(this IDeviceBuilder builder, MediatorConfigurator configurator)
        {
            DI.RegisterSingleton(typeof(IMediator), typeof(Mediator));
            var options = new MediatorOptions();
            configurator(options);
            DI.RegisterInstance(typeof(MediatorOptions), options);

            return builder;
        }
    }
}
