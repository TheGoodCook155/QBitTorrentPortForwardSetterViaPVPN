
using QBitTorrentPortForwardSetterViaPVPN.Constants;
using QBitTorrentPortForwardSetterViaPVPN.Models;
using System.Net;
using System.Net.Http;

namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class QBitTorrentCommander
    {
        private readonly QBitTorrentUserRetriever userRetriever;
        private HttpClient httpClient;
        private readonly PortForwardingFinder portForwardingFinder;

        public QBitTorrentCommander(
        QBitTorrentUserRetriever userRetriever,
        PortForwardingFinder portForwardingFinder)
        {
            this.userRetriever = userRetriever;

            this.portForwardingFinder = portForwardingFinder;

            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };

            this.httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost:8080")
            };
        }

        public async Task LoginToQBitTorrent()
        {
            QbitTorrentUserModel userModel = this.userRetriever.GetQbitTorrentUserCredentials();

            Dictionary<string, string> formData = new Dictionary<string, string>()
            {
                { "username", $"{userModel.Username}"},
                { "password", $"{userModel.Password}"}
            };

            var content = new FormUrlEncodedContent(formData);

            HttpResponseMessage response = await httpClient.PostAsync(QBitTorrentConstants.LoginEndpoint, content);
            
            try
            {
                response.EnsureSuccessStatusCode();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while logging to QBitTorrent Client");
            }

            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Result code: {result}");
        }

        public async Task SetForwardedPort()
        {
            string port = this.portForwardingFinder.GetForwardedPort();

            Dictionary<string, string> formData = new Dictionary<string, string>()
            {
                { "json", $"{{\"listen_port\":{port}}}" }
            };

            var content = new FormUrlEncodedContent(formData);

            HttpResponseMessage response = await httpClient.PostAsync(QBitTorrentConstants.SetPreferencesEndpoint, content);

            try
            {
                response.EnsureSuccessStatusCode();

            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error while assigning new port to QBitTorrent Client");            
            }

            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Result code: {result}");

        }
    }
}
