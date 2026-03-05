using Microsoft.Extensions.DependencyInjection;
using QBitTorrentPortForwardSetterViaPVPN.Constants;
using QBitTorrentPortForwardSetterViaPVPN.Helpers;
using QBitTorrentPortForwardSetterViaPVPN.Services;
using System.Net;

namespace QBitTorrentPortForwardSetterViaPVPN.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApp(this IServiceCollection @this)
        {
            @this.AddScoped<App>();
            return @this;
        }

        public static IServiceCollection AddLogCopyService(this IServiceCollection @this)
        {
            @this.AddScoped<PvpnLogCopy>();
            return @this;
        }


        public static IServiceCollection AddPathConstants(this IServiceCollection @this)
        {
            @this.AddScoped<PathConstants>();
            return @this;
        }


        public static IServiceCollection AddLogsHelpers(this IServiceCollection @this)
        {
            @this.AddScoped<LogsHelper>();
            return @this;
        }


        public static IServiceCollection AddPortForwardedFinder(this IServiceCollection @this)
        {
            @this.AddScoped<PortForwardingFinder>();
            return @this;
        }

        public static IServiceCollection AddQbitTorrentUserRetriever(this IServiceCollection @this)
        {
            @this.AddScoped<IQBitTorrentUserRetriever, QBitTorrentUserRetriever>();
            return @this;
        }

        public static IServiceCollection AddQbitTorrentCommander(this IServiceCollection @this)
        {
            @this.AddScoped<QBitTorrentCommander>();
            return @this;
        }

        public static IServiceCollection AddHttpClient(this IServiceCollection @this)
        {
            @this.AddSingleton<HttpClient>(serviceProvider =>
            {
                var handler = new HttpClientHandler
                {
                    UseCookies = true,
                    CookieContainer = new CookieContainer()
                };

                return new HttpClient(handler)
                {
                    BaseAddress = new Uri(QBitTorrentConstants.BaseAddress)
                };
            });

            return @this;
        }
    }
}
