
namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class App
    {
        private readonly IPvpnLogCopy logCopy;
        private readonly IPortForwardingFinder portForwardingFinder;
        private readonly IQBitTorrentUserRetriever userRetriever;
        private readonly IQBitTorrentCommander commander;
        public virtual string OldAssignedPort { get; set; } = string.Empty;
        private CancellationTokenSource cancellationTokenSource;

        public App(IPvpnLogCopy logCopy,
            IPortForwardingFinder portForwardingFinder,
            IQBitTorrentUserRetriever userRetriever,
            IQBitTorrentCommander commander,
            CancellationTokenSource cancellationTokenSource)
        {
            this.logCopy = logCopy;
            this.portForwardingFinder = portForwardingFinder;
            this.userRetriever = userRetriever;
            this.commander = commander;
            this.cancellationTokenSource = cancellationTokenSource;
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

                if (OldAssignedPort != newPort)
                {
                    Console.WriteLine($"Last port change found: {OldAssignedPort} -> {newPort}");

                    OldAssignedPort = newPort;
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
