using QBitTorrentPortForwardSetterViaPVPN.Models;

namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public interface IQBitTorrentUserRetriever
    {
        public QbitTorrentUserModel GetQbitTorrentUserCredentials();
    }
}
