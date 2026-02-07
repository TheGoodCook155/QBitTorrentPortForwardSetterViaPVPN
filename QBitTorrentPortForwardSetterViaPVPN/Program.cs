using Microsoft.Extensions.DependencyInjection;
using QBitTorrentPortForwardSetterViaPVPN.Extensions;
using QBitTorrentPortForwardSetterViaPVPN.Services;


namespace QBitTorrentPortForwardSetterViaPVPN
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services
                .AddApp()
                .AddLogCopyService()
                .AddPathConstants()
                .AddPvpnFolderMonitor()
                .AddLogsHelpers()
                .AddPortForwardedFinder()
                .AddQbitTorrentUserRetriever();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            App app = serviceProvider.GetRequiredService<App>();

            await app.Run();
        }
    }
}
