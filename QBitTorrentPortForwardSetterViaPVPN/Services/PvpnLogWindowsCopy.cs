using QBitTorrentPortForwardSetterViaPVPN.Constants;
using QBitTorrentPortForwardSetterViaPVPN.Helpers;
using QBitTorrentPortForwardSetterViaPVPN.Services;
using System.Diagnostics;

public sealed class PvpnLogWindowsCopy : PvpnLogCopyBase
{
    public PvpnLogWindowsCopy(PathConstants pathConstants, LogsHelper logsHelper) 
        : base(pathConstants, logsHelper)
    {
    }

    public override void CopyLogsToProject(bool overwrite = true)
    {
        base.CopyLogsToProject(overwrite);

        CopyWithXCopy(overwrite);
    }

    private void CopyWithXCopy(bool overwrite)
    {
        try
        {
            string xcopyArgs = $"\"{source}\" \"{destination}\" /E /I /C /R";

            if (overwrite)
            {
                xcopyArgs += " /Y";
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "xcopy.exe",
                Arguments = xcopyArgs,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (Process process = new Process())
            {
                process.StartInfo = processStartInfo;

                process.Start();

                process.BeginOutputReadLine();

                process.BeginErrorReadLine();

                if (!process.WaitForExit(60000))
                {
                    process.Kill();

                    Console.WriteLine("XCopy timed out after 60 seconds.");

                    return;
                }

                int exitCode = process.ExitCode;

                if (exitCode != 0 && exitCode != 1)
                {
                    Console.WriteLine($"XCopy may have encountered errors. Exit code: {exitCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"XCopy failed: {ex.Message}");
        }
    }
}