
using QBitTorrentPortForwardSetterViaPVPN.Constants;
using QBitTorrentPortForwardSetterViaPVPN.Helpers;
using Timer = System.Timers.Timer;

namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class PvpnFolderMonitor
    {
        private FileSystemWatcher watcher;
        public event Func<Object, FileSystemEventArgs,Task> OnLogsChanged;
        private readonly LogsHelper logsHelper;
        private readonly PathConstants pathConstants;
        private Timer timer;

        public PvpnFolderMonitor(LogsHelper logsHelper, PathConstants pathConstants)
        {
            this.logsHelper = logsHelper;
            this.pathConstants = pathConstants;
            this.InitTimer();
        }

        private void InitTimer()
        {
            timer = new Timer(10000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("10 sec passed");

            this.ForceLogFlush();
        }

        private void ForceLogFlush()
        {
            Console.WriteLine("Force log flush");

            string[] protonVpnLogs = this.logsHelper.RetrieveLogs(pathConstants.PvpnLogsPath);

            FileStream fileStream = null;
            
            foreach (var logFile in protonVpnLogs)
            {

                try
                {
                    fileStream = new FileStream(logFile,FileMode.Open,FileAccess.ReadWrite,FileShare.ReadWrite);
                    fileStream.Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unable to open file: {e.Message}"); ;
                }
                finally 
                { 
                    if (fileStream is not null)
                    {
                        fileStream?.Close(); 
                    }
                }
            }
        }

        public void InitWatcher(string folderPath)
        {
            Console.WriteLine("Init folder monitor");

            this.watcher = new FileSystemWatcher
            {
                Path = folderPath,
                Filter = "*.txt",
                IncludeSubdirectories = false,
                NotifyFilter = NotifyFilters.LastWrite |
                          NotifyFilters.FileName |
                          NotifyFilters.Size |
                          NotifyFilters.DirectoryName |
                          NotifyFilters.CreationTime,
            };

            Console.WriteLine($"Monitoring folder: {folderPath}");

            this.SubscribeToEvents(watcher);

            this.watcher.EnableRaisingEvents = true;
        }

        private void OnFileChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("Folder monitor | on FileChanged");
            this.OnLogsChanged.Invoke(source,e);
        }

        private void OnFileCreated(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("Folder monitor | on FileCreated");

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
