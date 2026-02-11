using QBitTorrentPortForwardSetterViaPVPN.Constants;
using QBitTorrentPortForwardSetterViaPVPN.Helpers;
using QBitTorrentPortForwardSetterViaPVPN.Services;
using System.Diagnostics;

public class PvpnLogCopy
{
    private string source;
    private string destination;
    private string projectPath;
    public string SourceDirectory => source;
    public string DestinationDirectory => destination;
    public string ProjectPath => projectPath;

    private readonly PathConstants pathConstants;
    private readonly LogsHelper logHelpers;

    public PvpnLogCopy(PathConstants pathConstants, LogsHelper logsHelper)
    {
        this.pathConstants = pathConstants;

        this.logHelpers = logsHelper;

        this.InitSource();

        this.InitDestination(projectPath!);
    }

    private void InitSource()
    {
        this.source = pathConstants.PvpnLogsPath;
    }

    private void InitDestination(string projectPath = "")
    {
        if (string.IsNullOrEmpty(projectPath))
        {
            projectPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        }

        this.destination = Path.Combine(projectPath, "VPN_Logs");
        this.projectPath = projectPath;

        Directory.CreateDirectory(destination);
    }

    public void CopyLogsToProject(bool overwrite = true)
    {
        string[] allLogFiles = this.logHelpers.RetrieveLogs(this.source);

        if (allLogFiles.Length == 0)
        {
            return;
        }
  
        CopyWithXCopy(allLogFiles, overwrite);
    }

    private void CopyWithXCopy(string[] files, bool overwrite)
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