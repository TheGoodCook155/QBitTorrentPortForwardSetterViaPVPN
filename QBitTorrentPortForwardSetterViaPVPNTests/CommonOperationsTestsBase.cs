
using QBitTorrentPortForwardSetterViaPVPN.Constants;
using QBitTorrentPortForwardSetterViaPVPN.Helpers;
using QBitTorrentPortForwardSetterViaPVPN.Services;

namespace QBitTorrentPortForwardSetterViaPVPNTests
{
    public class CommonOperationsTestsBase
    {
        protected string logEntry = string.Empty;
        
        protected  static string olderFile = "olderFile.txt";
        
        protected  static string newerFile = "newerFile.txt";

        protected PathConstants pathConstants;

        protected PortForwardingFinder portForwardingFinder;

        protected LogsHelper logsHelper;

        protected void InitPathConstants(Func<string> act)
        {
            pathConstants = new PathConstants();

            var basePath = AppContext.BaseDirectory;

            var fullPath = Path.Combine(basePath, act());

            pathConstants.ProjectPath = fullPath;

        }

        private void InitLogsHelper()
        {
            this.logsHelper = new LogsHelper();
        }

        protected void InitPathConstants(string path)
        {
            pathConstants = new PathConstants();

            pathConstants.ProjectPath = path;
        }

        protected void RemoveFiles(string path, IEnumerable<string> files) 
        {
            foreach (var file in files) 
            {
                string filePath = Path.Combine(path, file);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        private void Init()
        {
            this.InitPathConstants(() => "Fake_Logs");

            this.InitLogsHelper();

            portForwardingFinder = new PortForwardingFinder(this.pathConstants, this.logsHelper);
        }

        protected string SetLogEntry(string utcTime, string fromPort, string toPort)
        {
            return logEntry = $"{utcTime} | INFO  | APP | Port forwarding port changed from '{fromPort}' to '{toPort}'. |" + "{\"Caller\":\"PortForwardingManager.Receive:79\"}" + "\n";
        }

        protected async Task AppendToLogFile(string logFilePath, string content)
        {
            await File.AppendAllTextAsync(logFilePath, content);
        }

        protected void CreateFolderINotExist(string path, string directoryName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        protected void RemoveFolder(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
        }

        public CommonOperationsTestsBase()
        {
            this.Init();
        }
    }
}
