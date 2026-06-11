
namespace QBitTorrentPortForwardSetterViaPVPN.Constants
{
    public class PathConstants
    {
        public string? LocalApplicationData { get; set; } 
        public string? PvpnLogsPath { get; set; }

        public string ProjectPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "VPN_Logs");

        public static Os OsVersion { get; private set; }

        public PathConstants()
        {
            SetOsVersion();

            SetPathConstants();
        }

        private void SetPathConstantsForWindows() 
        {
            this.LocalApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this.PvpnLogsPath = Path.Combine(LocalApplicationData, "Proton", "Proton VPN", "Logs");
        }

        private void SetPathConstantsForMacOs()
        {
            //TODO, find more possible paths
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            string[] possiblePaths =
            {
                     Path.Combine(home,
                         "Library",
                         "Application Support",
                         "ch.protonvpn.mac",
                         "Logs"),

                     Path.Combine(home,
                         "Library",
                         "Containers",
                         "ProtonVPN",
                         "Data",
                         "Library",
                         "Logs")
            };

            string? logPath = possiblePaths
                            .FirstOrDefault(Directory.Exists);

            this.LocalApplicationData = string.Empty;

            this.PvpnLogsPath = logPath;
        }

        private void SetPathConstants() 
        { 
            switch (OsVersion) 
            {
                case Os.Windows:
                    this.SetPathConstantsForWindows();
                    break;
                case Os.MacOs:
                    this.SetPathConstantsForMacOs();
                    break;
                case Os.Linux:
                    throw new UnsupportedOsException("Operating system is not supported");
                default:
                    throw new UnsupportedOsException("Operating system is not supported");

            }

        }

        private void SetOsVersion() 
        {
            if (OperatingSystem.IsWindows()) 
            {
                OsVersion = Os.Windows;
            }else if (OperatingSystem.IsLinux()) 
            {
                OsVersion = Os.Linux;
            }else if (OperatingSystem.IsMacOS())
            {
                OsVersion = Os.MacOs;
            }
        }
    }
}
