using Microsoft.Extensions.DependencyInjection;
using QBitTorrentPortForwardSetterViaPVPN.Constants;
using QBitTorrentPortForwardSetterViaPVPN.Helpers;
using QBitTorrentPortForwardSetterViaPVPN.Services;

namespace QBitTorrentPortForwardSetterViaPVPN.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceCollection AddApp(this ServiceCollection @this)
        {
            @this.AddSingleton<App>();
            return @this;
        }

        public static ServiceCollection AddLogCopyService(this ServiceCollection @this)
        {
            @this.AddScoped<PvpnLogCopy>();
            return @this;
        }


        public static ServiceCollection AddPathConstants(this ServiceCollection @this)
        {
            @this.AddScoped<PathConstants>();
            return @this;
        }


        public static ServiceCollection AddPvpnFolderMonitor(this ServiceCollection @this)
        {
            @this.AddSingleton<PvpnFolderMonitor>();
            return @this;
        }


        public static ServiceCollection AddLogsHelpers(this ServiceCollection @this)
        {
            @this.AddScoped<LogsHelper>();
            return @this;
        }


        public static ServiceCollection AddPortForwardedFinder(this ServiceCollection @this)
        {
            @this.AddScoped<PortForwardingFinder>();
            return @this;
        }

        public static ServiceCollection AddQbitTorrentUserRetriever(this ServiceCollection @this)
        {
            @this.AddScoped<QBitTorrentUserRetriever>();
            return @this;   
        }

        public static ServiceCollection AddQbitTorrentCommander(this ServiceCollection @this)
        {
            @this.AddScoped<QBitTorrentCommander>();
            return @this;
        }
    }
}
