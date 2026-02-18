
namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class PvpnFolderMonitor
    {
        private FileSystemWatcher watcher;
        public event Func<Object, FileSystemEventArgs,Task> OnLogsChanged;

        public PvpnFolderMonitor()
        {
        }

        public void InitWatcher(string folderPath)
        {
            this.watcher = new FileSystemWatcher(folderPath)
            {
                Filter = "*.txt",
                IncludeSubdirectories = false,
                NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size
            };

            Console.WriteLine($"Monitoring folder: {folderPath}");

            this.SubscribeToEvents(watcher);

            this.watcher.EnableRaisingEvents = true;
        }

        private void OnFileChanged(object source, FileSystemEventArgs e)
        {
            this.OnLogsChanged?.Invoke(source,e);
        }

        private void OnFileCreated(object source, FileSystemEventArgs e)
        {
            this.OnLogsChanged?.Invoke(source, e);
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
