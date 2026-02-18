
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

        public QBitTorrentCommander(
        QBitTorrentUserRetriever userRetriever)
        {
            this.userRetriever = userRetriever;

            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };

            this.httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(QBitTorrentConstants.BaseAddress)
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

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(QBitTorrentConstants.LoginEndpoint, content);
            
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Log in to qBittorrent Succesfull");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while logging to qBittorrent Client");
            }

        }

        public async Task SetForwardedPort(string port)
        {
            Dictionary<string, string> formData = new Dictionary<string, string>()
            {
                { "json", $"{{\"listen_port\":{port}}}" }
            };

            var content = new FormUrlEncodedContent(formData);

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(QBitTorrentConstants.SetPreferencesEndpoint, content);

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Port set in qBittorrent");

            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error while assigning new port to qBittorrent Client");            
            }

        }
    }
}
