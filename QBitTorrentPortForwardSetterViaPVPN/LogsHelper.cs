using System;
using System.Collections.Generic;
using System.Text;

namespace QBitTorrentPortForwardSetterViaPVPN
{
    public class LogsHelper
    {
        public string[] RetrieveLogs(string source)
        {
            try
            {
                if (!Directory.Exists(source))
                {
                    throw new Exception($"Source directory not found: {source}");
                }

                string[] files = Directory.GetFiles(source, "*.txt", SearchOption.AllDirectories)
                                         .ToArray();
                return files;
            }
            catch (Exception ex)
            {
                Console.WriteLine("PVPN logs were not found");
            }

            return [];
        }
    }
}
