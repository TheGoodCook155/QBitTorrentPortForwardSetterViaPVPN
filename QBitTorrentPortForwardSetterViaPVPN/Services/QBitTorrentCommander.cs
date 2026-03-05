
using QBitTorrentPortForwardSetterViaPVPN.Constants;
using QBitTorrentPortForwardSetterViaPVPN.Models;

namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class QBitTorrentCommander
    {
        private readonly IQBitTorrentUserRetriever userRetriever;
        private HttpClient httpClient;
        private readonly PortForwardingFinder portForwardingFinder;

        public QBitTorrentCommander(
        IQBitTorrentUserRetriever userRetriever,
        PortForwardingFinder portForwardingFinder,
        HttpClient httpClient)
        {
            this.userRetriever = userRetriever;

            this.portForwardingFinder = portForwardingFinder;

            this.httpClient = httpClient;
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

                Console.WriteLine($"Port set in qBittorrent!");

            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error while assigning new port to qBittorrent Client");            
            }

        }
    }
}
