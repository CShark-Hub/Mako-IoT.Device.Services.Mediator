using MakoIoT.Device.Services.Interface;
using nanoFramework.DependencyInjection;

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
            builder.Services.AddSingleton(typeof(IMediator), typeof(Mediator));
            var options = new MediatorOptions();
            configurator(options);
            builder.Services.AddSingleton(typeof(MediatorOptions), options);

            return builder;
        }
    }
}
