using FluentAssertions;
using QBitTorrentPortForwardSetterViaPVPN.Helpers;

namespace QBitTorrentPortForwardSetterViaPVPNTests.Helpers
{
    public class LogsHelperTests : CommonOperationsTestsBase
    {
        private LogsHelper logsHelper = new LogsHelper();

        [Theory]
        [MemberData(nameof(LogsHelperTestData))]
        public async Task RetrieveLogs_WithValidPath_ReturnsLogFiles_OrderedByLastWriteTimeUtc(List<string> testFiles, List<string> expectedOrder) 
        {
            string logsPath = this.pathConstants.ProjectPath;

            string filePath = string.Empty;

            foreach (var testFile in testFiles) 
            {
                filePath = Path.Combine(logsPath, testFile);

                File.WriteAllText(filePath, "Test text");
            }

            await Task.Delay(1000);

           string [] logFiles = this.logsHelper.RetrieveLogs(logsPath);

            IEnumerable<string> logFilesWithRemovedTestLogs = logFiles
                .Select(Path.GetFileName)
                .Where(el=> testFiles.Contains(el));

            logFilesWithRemovedTestLogs.Should().BeEquivalentTo(expectedOrder);

            this.RemoveFiles(logsPath, testFiles);
        }

        #region LogsHelperTestData
        public static IEnumerable<object[]> LogsHelperTestData =>
           new List<object[]>()
           {
                new object[]
                {
                    new List<string>(){"1.txt","2.txt","3.txt","4.txt","5.txt" },
                    new List<string>(){"1.txt","2.txt","3.txt","4.txt","5.txt" }
                },
                new object[]
                {
                    new List<string>(){"2.txt","1.txt","3.txt","4.txt","5.txt" },
                    new List<string>(){"2.txt","1.txt","3.txt","4.txt","5.txt" }
                },
                new object[]
                {
                    new List<string>(){"5.txt","4.txt","3.txt","2.txt","1.txt" },
                    new List<string>(){"5.txt","4.txt","3.txt","2.txt","1.txt" }
                },
                new object[]
                {
                    new List<string>(){"1.log","2.log","3.log","4.log","5.log" },
                    new List<string>(){"1.log","2.log","3.log","4.log","5.log" }
                },
                new object[]
                {
                    new List<string>(){"2.log","1.log","3.log","4.log","5.log" },
                    new List<string>(){"2.log","1.log","3.log","4.log","5.log" }
                },
                new object[]
                {
                    new List<string>(){"5.log","4.log","3.log","2.log","1.log" },
                    new List<string>(){"5.log","4.log","3.log","2.log","1.log" }
                },
           };

        #endregion
   }
}
