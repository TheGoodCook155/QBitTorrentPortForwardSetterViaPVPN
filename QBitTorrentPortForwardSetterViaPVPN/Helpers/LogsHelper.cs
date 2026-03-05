using System;
using System.Collections.Generic;
using System.Text;

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
                    .GetFiles(source, "*.txt", SearchOption.AllDirectories)
                    .OrderBy(f => File.GetLastWriteTimeUtc(f))
                    .ToArray();
        }
    }
}
