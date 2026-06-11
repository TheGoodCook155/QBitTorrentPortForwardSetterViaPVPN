
using QBitTorrentPortForwardSetterViaPVPN.Constants;
using QBitTorrentPortForwardSetterViaPVPN.Helpers;

namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class PvpnLogCopyBase : IPvpnLogCopy
    {
        protected string source;
        protected string destination;
        protected string projectPath;
        public string SourceDirectory => source;
        public string DestinationDirectory => destination;
        public string ProjectPath => projectPath;

        protected  PathConstants pathConstants;
        protected  LogsHelper logHelpers;

        public PvpnLogCopyBase(PathConstants pathConstants, LogsHelper logsHelper)
        {
            this.pathConstants = pathConstants;

            this.logHelpers = logsHelper;

            this.InitSource();

            this.InitDestination(projectPath!);
        }

        protected void InitSource()
        {
            this.source = pathConstants.PvpnLogsPath!;
        }

        protected void InitDestination(string projectPath = "")
        {
            if (string.IsNullOrEmpty(projectPath))
            {
                projectPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            }

            this.destination = Path.Combine(projectPath, "VPN_Logs");
            this.projectPath = projectPath;

            Directory.CreateDirectory(destination);
        }

        public virtual void CopyLogsToProject(bool overwrite = true)
        {
            string[] allLogFiles = this.logHelpers.RetrieveLogs(this.source);

            if (allLogFiles.Length == 0)
            {
                return;
            }
        }
    }
}
