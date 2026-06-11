
using QBitTorrentPortForwardSetterViaPVPN.Constants;
using QBitTorrentPortForwardSetterViaPVPN.Helpers;

namespace QBitTorrentPortForwardSetterViaPVPN.Services
{
    public sealed class PvpnLogLinuxCopy : PvpnLogCopyBase
    {
        public PvpnLogLinuxCopy(PathConstants pathConstants, LogsHelper logsHelper)
       : base(pathConstants, logsHelper)
        {
        }

        public override void CopyLogsToProject(bool overwrite = true)
        {
            base.CopyLogsToProject(overwrite);

            CopyOnLinux();
        }

        private void CopyOnLinux()
        {
            string[] files = Directory.GetFiles(this.source);

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
                catch (Exception e)
                {
#if DEBUG
                    Console.WriteLine(e.Message.ToString());
#endif
                }
            }
        }
    }
}
