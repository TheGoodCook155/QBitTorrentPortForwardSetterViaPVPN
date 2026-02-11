
namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class App
    {
        private readonly PvpnLogCopy logCopy;
        private readonly PvpnFolderMonitor folderMonitor;
        private readonly PortForwardingFinder portForwardingFinder;
        private readonly QBitTorrentUserRetriever userRetriever;
        private readonly QBitTorrentCommander commander;
        private string oldAssignedPort = string.Empty;

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

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

        private void Cleanup()
        {
            folderMonitor.Stop();
            this.logCopy.Stop();
        }

        public async Task Run()
        {

            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Shutting down...");
                cancellationTokenSource.Cancel();
                e.Cancel = true;
                this.Cleanup();
            };

            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                logCopy.CopyLogsToProject();

                string newPort = this.portForwardingFinder.GetForwardedPort();

                if (string.IsNullOrEmpty(newPort))
                {
                    await Task.Delay(10000);
                    continue;
                }

                if (oldAssignedPort != newPort)
                {
                    Console.WriteLine($"Last port change found: {oldAssignedPort} -> {newPort}");

                    oldAssignedPort = newPort;
                }
                else
                {
                    await Task.Delay(10000);
                    continue;
                }

                this.userRetriever.GetQbitTorrentUserCredentials();

                await this.commander.LoginToQBitTorrent();

                await this.commander.SetForwardedPort(newPort);

                await Task.Delay(10000);
            }

            Console.WriteLine("Done");
        }
    }
}
