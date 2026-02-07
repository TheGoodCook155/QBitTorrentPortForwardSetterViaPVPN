using QBitTorrentPortForwardSetterViaPVPN.Models;
using System.Text.Json;

namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class QBitTorrentUserRetriever
    {
        public QbitTorrentUserModel GetQbitTorrentUserCredentials()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            string appSettingsPath = Path.Combine(currentDirectory,"appsettings.json");

            string configContent = File.ReadAllText(appSettingsPath);

            QbitTorrentUserModel qbitTorrentUser = JsonSerializer.Deserialize<QbitTorrentUserModel>(configContent);

            return qbitTorrentUser;
        }
    }
}
