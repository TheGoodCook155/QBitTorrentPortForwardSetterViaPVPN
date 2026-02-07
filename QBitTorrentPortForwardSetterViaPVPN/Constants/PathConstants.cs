
namespace QBitTorrentPortForwardSetterViaPVPN.Constants
{
    public class PathConstants
    {
        public string LocalApplicationData { get; set; } 
        public string PvpnLogsPath { get; set; }

        public string ProjectPath { get; set; }

        public PathConstants()
        {
            this.LocalApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this.PvpnLogsPath = Path.Combine(LocalApplicationData, "Proton", "Proton VPN", "Logs");
            ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "VPN_Logs");
        }
    }
}
