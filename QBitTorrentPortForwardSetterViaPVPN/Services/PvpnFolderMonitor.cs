
using QBitTorrentPortForwardSetterViaPVPN.Constants;

namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class PvpnFolderMonitor : IDisposable
    {
        private FileSystemWatcher watcher;
        private PathConstants pathConstants;
        private string folderPath;
        public event EventHandler OnLogsChanged;
        public bool isAlive = true;


        public PvpnFolderMonitor( PathConstants pathConstants)
        {
            Console.WriteLine("PvpnFolder monitor instantiated");

            this.pathConstants = pathConstants;

            folderPath = pathConstants.PvpnLogsPath;

            this.InitWatcher(folderPath);
        }

        private void InitWatcher(string folderPath)
        {
            this.watcher = new FileSystemWatcher
            {
                Path = folderPath,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = "*.txt"
            };

            this.SubscribeToEvents(watcher);

            watcher.EnableRaisingEvents = true;
        }

        private void OnFileChanged(object source, FileSystemEventArgs e)
        {
            this.OnLogsChanged.Invoke(this, e);
        }

        private void OnFileCreated(object source, FileSystemEventArgs e)
        {
            this.OnLogsChanged.Invoke(this, e);
        }

        private void Stop()
        {
            watcher.EnableRaisingEvents = false;

            this.UnsubscribeToEvents(watcher);

            watcher.Dispose();

            this.isAlive = false;
        }

        private void UnsubscribeToEvents(FileSystemWatcher watcher)
        {
            watcher.Changed -= OnFileChanged;

            watcher.Created -= OnFileCreated;
        }

        private void SubscribeToEvents(FileSystemWatcher watcher)
        {
            watcher.Changed += OnFileChanged;

            watcher.Created += OnFileCreated;
        }

        public void Dispose()
        {
            Console.WriteLine("PvpnFolderMonitor disposed");
           this.Stop();
        }
    }
}
