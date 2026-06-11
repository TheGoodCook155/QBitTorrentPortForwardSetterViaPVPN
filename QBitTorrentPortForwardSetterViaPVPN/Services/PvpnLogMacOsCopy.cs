
using QBitTorrentPortForwardSetterViaPVPN.Constants;
using QBitTorrentPortForwardSetterViaPVPN.Helpers;

namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public sealed class PvpnLogMacOsCopy : PvpnLogCopyBase
    {
        public PvpnLogMacOsCopy(PathConstants pathConstants, LogsHelper logsHelper)
       : base(pathConstants, logsHelper)
        {
        }

        public override void CopyLogsToProject(bool overwrite = true)
        {
            base.CopyLogsToProject(overwrite);

            CopyOnMac();
        }

        private void CopyOnMac()
        {
            string [] files = Directory.GetFiles(this.source);

            foreach (string file in files) 
            {
                try 
                {
                    using var source = new FileStream(
                                        file,
                                        FileMode.Open,
                                        FileAccess.Read,
                                        FileShare.ReadWrite | FileShare.Delete);

                    using var destination = new FileStream(
                                            Path.Combine(this.destination, Path.GetFileName(file)),
                                            FileMode.Create,
                                            FileAccess.Write);

                    source.CopyTo(destination);
                }
                catch(Exception e) 
                {
#if DEBUG
                    Console.WriteLine(e.Message.ToString());
#endif
                }
            }
        }

    }
}
