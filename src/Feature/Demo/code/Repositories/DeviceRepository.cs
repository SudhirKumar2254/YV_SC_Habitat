namespace Sitecore.Feature.Demo.Repositories
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Analytics;
    using Sitecore.CES.DeviceDetection;
    using Sitecore.Feature.Demo.Models;
    using Sitecore.Foundation.DependencyInjection;

    [Service]
    public class DeviceRepository
    {
        private Device current;

        public Device GetCurrent()
        {
            if (this.current != null)
            {
                return this.current;
            }
            var deviceDetectionManager = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetRequiredService<Sitecore.CES.DeviceDetection.DeviceDetectionManagerBase>();

            if (!deviceDetectionManager.IsEnabled || !deviceDetectionManager.IsReady || string.IsNullOrEmpty(Tracker.Current.Interaction.UserAgent))
            {
                return null;
            }

            return this.current = this.CreateDevice(deviceDetectionManager.GetDeviceInformation(Tracker.Current.Interaction.UserAgent));
        }

        private Device CreateDevice(DeviceInformation deviceInformation)
        {
            return new Device
            {
                Title = string.Join(", ", deviceInformation.DeviceVendor, deviceInformation.DeviceModelName),
                Browser = deviceInformation.Browser
            };
        }
    }
}