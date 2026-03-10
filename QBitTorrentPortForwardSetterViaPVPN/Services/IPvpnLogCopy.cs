public interface IPvpnLogCopy
{
    string DestinationDirectory { get; }
    string ProjectPath { get; }
    string SourceDirectory { get; }

    void CopyLogsToProject(bool overwrite = true);
}