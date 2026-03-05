using FluentAssertions;
using QBitTorrentPortForwardSetterViaPVPN.Constants;
using QBitTorrentPortForwardSetterViaPVPN.Services;

namespace QBitTorrentPortForwardSetterViaPVPNTests.Services
{
    public class PortForwardingFinderTests : CommonOperationsTestsBase
    {

        public PortForwardingFinderTests() : base()
        {
        }

        [Theory]
        [MemberData(nameof(PortForwardingTestData))]
        public async Task GetForwardedPort_MultipleFilesPresent_ReturnsThePort_From_LastChangedFile(
            string dateTime, 
            string fromPort,
            string toPort,
            string file,
            string expectedNewPort) 
        {
            string logEntry = SetLogEntry(dateTime, fromPort, toPort);

            string filePath = Path.Combine(pathConstants.ProjectPath, file);

            await AppendToLogFile(filePath,logEntry);

            await Task.Delay(1000);

            string retrievedPort = this.portForwardingFinder.GetForwardedPort();

            retrievedPort.Should().Be(expectedNewPort);
        }

        [Fact]
        public async Task GetForwardedPort_SourcePathInvalid_ThrowsException() 
        {
            this.InitPathConstants("invalid_path");

            portForwardingFinder = new PortForwardingFinder(this.pathConstants, this.logsHelper);

            Action act = () => this.portForwardingFinder.GetForwardedPort();

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void GetForwardedPort_SourcePathValid_NoLogFilesArePresent_ReturnsEmptyStringPort()
        {
            this.InitPathConstants(() => "PVPN_FakeLogs");

            CreateFolderINotExist(this.pathConstants.ProjectPath,"PVPN_FakeLogs");

            portForwardingFinder = new PortForwardingFinder(this.pathConstants, this.logsHelper);

            string forwardedPort = portForwardingFinder.GetForwardedPort();

            forwardedPort.Should().Be("");

            this.RemoveFolder(this.pathConstants.ProjectPath);
        }

        [Fact]
        public async Task GetForwardedPort_InvalidLogEntry_ThrowsException() 
        {
            string fileName = "log.txt";

            File.WriteAllText(Path.Combine(this.pathConstants.ProjectPath,fileName), GeneralConstants.PvpnLogPortEntry);

            Action act = () => this.portForwardingFinder.GetForwardedPort();

            act.Should().Throw<Exception>();

            if(File.Exists(Path.Combine(this.pathConstants.ProjectPath, fileName))) 
            {
                File.Delete(Path.Combine(this.pathConstants.ProjectPath, fileName));
            }
        }

        #region TestData
        public static IEnumerable<object[]> PortForwardingTestData =>
            new List<object[]>()
            {
                new object[]
                {
                    DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),"","23456",olderFile,"23456"
                },
                new object[]
                {
                    DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),"12345","23456",olderFile,"23456"
                },
                new object[]
                {
                    DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),"","",olderFile,""
                },
                new object[]
                {
                    DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),"12345","",olderFile, ""
                },
                new object[]
                {
                    DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),"","23456",newerFile,"23456"
                },
                new object[]
                {
                    DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),"12345","23456",newerFile,"23456"
                },
                new object[]
                {
                    DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),"","",newerFile,""
                },
                new object[]
                {
                    DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),"12345","",newerFile, ""
                }
            };
        #endregion
    }
}
