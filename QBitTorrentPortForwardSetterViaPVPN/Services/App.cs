
namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class App
    {
        private readonly PvpnLogCopy logCopy;
        private readonly PvpnFolderMonitor folderMonitor;
        private readonly PortForwardingFinder portForwardingFinder;

        public App(PvpnLogCopy logCopy,
            PvpnFolderMonitor folderMonitor,
            PortForwardingFinder portForwardingFinder)
        {
            this.logCopy = logCopy;
            this.folderMonitor = folderMonitor;
            this.portForwardingFinder = portForwardingFinder;
        }

        public async Task Run()
        {
            logCopy.CopyLogsToProject();

            while (folderMonitor.isAlive)
            {
                // Get th port via service
                string newPort = this.portForwardingFinder.GetForwardedPort();

                //Asign the port to QbitTorrent

                await Task.Delay(20000);
            }

            Console.WriteLine("Done");

        }
    }
}
