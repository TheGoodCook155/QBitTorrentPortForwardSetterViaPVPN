
namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class App
    {
        private readonly PvpnLogCopy logCopy;
        private readonly PvpnFolderMonitor folderMonitor;
        private readonly PortForwardingFinder portForwardingFinder;
        private readonly QBitTorrentUserRetriever userRetriever;
        private readonly QBitTorrentCommander commander;

        public App(PvpnLogCopy logCopy,
            PvpnFolderMonitor folderMonitor,
            PortForwardingFinder portForwardingFinder,
            QBitTorrentUserRetriever userRetriever,
            QBitTorrentCommander commander)
        {
            this.logCopy = logCopy;
            this.folderMonitor = folderMonitor;
            this.portForwardingFinder = portForwardingFinder;
            this.userRetriever = userRetriever;
            this.commander = commander;
        }

        public async Task Run()
        {
            logCopy.CopyLogsToProject();

            while (folderMonitor.isAlive)
            {
                string newPort = this.portForwardingFinder.GetForwardedPort();

                this.userRetriever.GetQbitTorrentUserCredentials();

                await this.commander.LoginToQBitTorrent();

                await this.commander.SetForwardedPort();

                await Task.Delay(20000);
            }

            Console.WriteLine("Done");

        }
    }
}
