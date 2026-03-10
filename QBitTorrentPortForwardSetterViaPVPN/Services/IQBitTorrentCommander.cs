namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public interface IQBitTorrentCommander
    {
        Task LoginToQBitTorrent();
        Task SetForwardedPort(string port);
    }
}