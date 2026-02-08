
using QBitTorrentPortForwardSetterViaPVPN.Constants;

namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class PvpnFolderMonitor
    {
        private FileSystemWatcher watcher;
        private PathConstants pathConstants;
        private string folderPath;
        public event EventHandler OnLogsChanged;


        public PvpnFolderMonitor( PathConstants pathConstants)
        {
            this.pathConstants = pathConstants;

            folderPath = pathConstants.PvpnLogsPath;

            this.InitWatcher(folderPath);
        }

        private void InitWatcher(string folderPath)
        {
            this.watcher = new FileSystemWatcher
            {
                Path = folderPath,
                Filter = "*.txt",
                IncludeSubdirectories = false,
                NotifyFilter = NotifyFilters.LastWrite |
                          NotifyFilters.FileName |
                          NotifyFilters.Size,
                EnableRaisingEvents = true
            };

            Console.WriteLine($"Monitoring folder: {folderPath}");

            this.SubscribeToEvents(watcher);
        }

        private void OnFileChanged(object source, FileSystemEventArgs e)
        {
            this.OnLogsChanged.Invoke(this, e);
        }

        private void OnFileCreated(object source, FileSystemEventArgs e)
        {
            this.OnLogsChanged.Invoke(this, e);
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;

            this.UnsubscribeToEvents(watcher);

            watcher.Dispose();
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
    }
}
