using QBitTorrentPortForwardSetterViaPVPN.Constants;
using System.Text.RegularExpressions;

namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public class PortForwardingFinder
    {

        private readonly PathConstants pathConstants;
        private readonly LogsHelper logHelper;  

        public PortForwardingFinder(PathConstants pathConstants, LogsHelper logHelper)
        {
            this.pathConstants = pathConstants;
            this.logHelper = logHelper;
        }

        public string GetForwardedPort()
        {
            string newPort = GetPort();

            return newPort;
        }

        private string[] GetLogFiles()
        {
            string path = pathConstants.ProjectPath;
            
            string[] allFiles = logHelper.RetrieveLogs(path);

            return allFiles;
        }

        private string GetPort()
        {
            List<string> allPortEntries = new List<string>();

            string[] logFiles = GetLogFiles();

            foreach (string file in logFiles)
            {
                try
                {
                    string[] lines = File.ReadAllLines(file);

                    foreach (string line in lines)
                    {
                        if (line.Contains("Port forwarding port changed"))
                        {
                            allPortEntries.Add(line);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file {file}: {ex.Message}");
                }
            }

            string lastPortEntry = allPortEntries.LastOrDefault();

            if (string.IsNullOrEmpty(lastPortEntry))
            {
                Console.WriteLine("No port change entries found.");
                return null;
            }

            Match match = Regex.Match(lastPortEntry, @"from '(\d*)' to '(\d+)'");

            string oldPort = string.Empty;

            string newPort = string.Empty;

            if (match.Success)
            {
                oldPort = match.Groups[1].Value;
                newPort = match.Groups[2].Value;

                Console.WriteLine($"Last port change found: {oldPort} -> {newPort}");
                return newPort;
            }

            throw new Exception($"Could not parse port from entry:{lastPortEntry}");
        }
    }
}
