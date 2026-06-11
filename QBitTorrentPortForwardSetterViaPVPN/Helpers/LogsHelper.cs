
namespace QBitTorrentPortForwardSetterViaPVPN.Helpers
{
    public class LogsHelper
    {
        public string[] RetrieveLogs(string source)
        {

            if (!Directory.Exists(source))
            {
                throw new Exception($"Source directory not found: {source}");
            }

            return Directory
                    .GetFiles(source, "*", SearchOption.AllDirectories)
                    .Where(f =>
                    f.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) ||
                    f.EndsWith(".log", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(f => File.GetLastWriteTimeUtc(f))
                    .ToArray();
        }
    }
}
