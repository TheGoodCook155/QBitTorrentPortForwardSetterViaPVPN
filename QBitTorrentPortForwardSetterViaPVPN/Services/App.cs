
using QBitTorrentPortForwardSetterViaPVPN.Constants;
using System.Runtime.CompilerServices;

namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class App
    {
        private readonly PvpnLogCopy logCopy;
        private readonly PvpnFolderMonitor folderMonitor;
        private readonly PortForwardingFinder portForwardingFinder;
        private readonly QBitTorrentUserRetriever userRetriever;
        private readonly QBitTorrentCommander commander;
        private readonly PathConstants pathConstants;
        private string oldAssignedPort = string.Empty;

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public App(PvpnLogCopy logCopy,
            PvpnFolderMonitor folderMonitor,
            PortForwardingFinder portForwardingFinder,
            QBitTorrentUserRetriever userRetriever,
            QBitTorrentCommander commander,
            PathConstants pathConstants)
        {
            this.logCopy = logCopy;
            this.folderMonitor = folderMonitor;
            this.portForwardingFinder = portForwardingFinder;
            this.userRetriever = userRetriever;
            this.commander = commander;
            this.pathConstants = pathConstants;
        }

        private void InitFolderMonitor()
        {
            this.folderMonitor.InitWatcher(pathConstants.PvpnLogsPath);
        }

        private void SubscribeToFolderMonitor()
        {
            this.folderMonitor.OnLogsChanged += FolderMonitor_OnLogsChanged;
        }

        private async Task FolderMonitor_OnLogsChanged(object obj, FileSystemEventArgs args)
        {
            logCopy.CopyLogsToProject();

            string newPort = this.portForwardingFinder.GetForwardedPort();

            if (string.IsNullOrEmpty(newPort))
            {
                return;
            }

            if (this.oldAssignedPort != newPort)
            {
                Console.WriteLine($"Last port change found: {oldAssignedPort} -> {newPort}");
                
                this.oldAssignedPort = newPort;
            }

            this.userRetriever.GetQbitTorrentUserCredentials();

            await this.commander.LoginToQBitTorrent();

            await this.commander.SetForwardedPort(newPort);
        }

        private void Cleanup()
        {
            folderMonitor.Stop();
            this.folderMonitor.OnLogsChanged -= FolderMonitor_OnLogsChanged;
        }

        public async Task Run()
        {

            this.InitFolderMonitor();

            this.SubscribeToFolderMonitor();

            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Shutting down...");
                cancellationTokenSource.Cancel();
                e.Cancel = true;
                this.Cleanup();
            };

            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
            }
        }
    }
}
