
namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class App
    {
        private readonly PvpnLogCopy logCopy;
        private readonly PortForwardingFinder portForwardingFinder;
        private readonly IQBitTorrentUserRetriever userRetriever;
        private readonly QBitTorrentCommander commander;
        private string oldAssignedPort = string.Empty;

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public App(PvpnLogCopy logCopy,
            PortForwardingFinder portForwardingFinder,
            IQBitTorrentUserRetriever userRetriever,
            QBitTorrentCommander commander)
        {
            this.logCopy = logCopy;
            this.portForwardingFinder = portForwardingFinder;
            this.userRetriever = userRetriever;
            this.commander = commander;
        }

        public async Task Run()
        {

            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Shutting down...");
                cancellationTokenSource.Cancel();
                e.Cancel = true;
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
