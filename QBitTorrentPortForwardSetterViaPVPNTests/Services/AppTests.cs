using Moq;
using QBitTorrentPortForwardSetterViaPVPN.Models;
using QBitTorrentPortForwardSetterViaPVPN.Services;

namespace QBitTorrentPortForwardSetterViaPVPNTests.Services
{
    public class AppTests
    {
        private readonly Mock<IPvpnLogCopy> logCopy = new Mock<IPvpnLogCopy>();

        private readonly Mock<IPortForwardingFinder> portForwardingFinder = new Mock<IPortForwardingFinder>();

        private readonly Mock<IQBitTorrentUserRetriever> userRetriever = new Mock<IQBitTorrentUserRetriever>();

        private readonly Mock<IQBitTorrentCommander> commander = new Mock<IQBitTorrentCommander>();

        private App app;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private void Init()
        {
            this.app = new App(logCopy.Object, portForwardingFinder.Object, userRetriever.Object, commander.Object, cts);
        }

        public AppTests()
        {
            this.Init();
        }

        [Theory]
        [MemberData(nameof(AppTestData))]
        public async Task Run_BasedOnRetrievedPort_MethodsAreExecuted_OrNot(
            string forwardedPort,
            Action<Mock<IPvpnLogCopy>> pvpnLogCopyMock,
            Action<Mock<IPortForwardingFinder>> portForwardingFinderMock,
            Action<Mock<IQBitTorrentUserRetriever>> userRetrieverMock,
            Action<Mock<IQBitTorrentCommander>> qbitTorrentCommanderMock,
            Action<Mock<IQBitTorrentCommander>, string> qbitTorrentCommanderSetPortMock,
            string oldAssignedPort)
        {
            this.logCopy.Setup(el => el.CopyLogsToProject()).Callback(()=> Console.WriteLine("Executed"));

            this.portForwardingFinder.Setup(el => el.GetForwardedPort()).Returns(forwardedPort);

            this.userRetriever.Setup(el => el.GetQbitTorrentUserCredentials()).Returns(new QbitTorrentUserModel() { Username = "testUser", Password = "testPass" });

            this.commander.Setup(el => el.LoginToQBitTorrent()).Returns(Task.CompletedTask);

            this.commander.Setup(el => el.SetForwardedPort(forwardedPort)).Returns(Task.CompletedTask);

            this.app.OldAssignedPort = oldAssignedPort;

            var runTask = this.app.Run();

            pvpnLogCopyMock?.Invoke(this.logCopy);

            portForwardingFinderMock?.Invoke(this.portForwardingFinder);

            userRetrieverMock?.Invoke(this.userRetriever);

            qbitTorrentCommanderMock?.Invoke(this.commander);

            qbitTorrentCommanderSetPortMock?.Invoke(this.commander, forwardedPort);

            await Task.Delay(1000);

            this.cts.Cancel();

            await runTask;
        }

        #region TestData
        public static IEnumerable<object[]> AppTestData =>
            new List<object[]> 
            {
                //retrieved port is empty string or null, some methods are not executed
                new object[]
                {
                    "",
                    new Action<Mock<IPvpnLogCopy>>((arg) => arg.Verify(el=> el.CopyLogsToProject(),Times.Once)),
                    new Action<Mock<IPortForwardingFinder>>((arg) => arg.Verify(el=> el.GetForwardedPort(),Times.Once)),
                    new Action<Mock<IQBitTorrentUserRetriever>>((arg) => arg.Verify(el=> el.GetQbitTorrentUserCredentials(),Times.Never)),
                    new Action<Mock<IQBitTorrentCommander>>((arg) => arg.Verify(el=> el.LoginToQBitTorrent(),Times.Never)),
                    new Action<Mock<IQBitTorrentCommander>,string>((arg,port) => arg.Verify(el=> el.SetForwardedPort(port),Times.Never)),
                    ""
                },
                new object[]
                {
                    null!,
                    new Action<Mock<IPvpnLogCopy>>((arg) => arg.Verify(el=> el.CopyLogsToProject(),Times.Once)),
                    new Action<Mock<IPortForwardingFinder>>((arg) => arg.Verify(el=> el.GetForwardedPort(),Times.Once)),
                    new Action<Mock<IQBitTorrentUserRetriever>>((arg) => arg.Verify(el=> el.GetQbitTorrentUserCredentials(),Times.Never)),
                    new Action<Mock<IQBitTorrentCommander>>((arg) => arg.Verify(el=> el.LoginToQBitTorrent(),Times.Never)),
                    new Action<Mock<IQBitTorrentCommander>,string>((arg,port) => arg.Verify(el=> el.SetForwardedPort(port),Times.Never)),
                    ""
                },
               //retrieved port is different than old port all methods are executed
                new object[]
                {
                    "12345",
                    new Action<Mock<IPvpnLogCopy>>((arg) => arg.Verify(el=> el.CopyLogsToProject(),Times.Once)),
                    new Action<Mock<IPortForwardingFinder>>((arg) => arg.Verify(el=> el.GetForwardedPort(),Times.Once)),
                    new Action<Mock<IQBitTorrentUserRetriever>>((arg) => arg.Verify(el=> el.GetQbitTorrentUserCredentials(),Times.Once)),
                    new Action<Mock<IQBitTorrentCommander>>((arg) => arg.Verify(el=> el.LoginToQBitTorrent(),Times.Once)),
                    new Action<Mock<IQBitTorrentCommander>,string>((arg,port) => arg.Verify(el=> el.SetForwardedPort(port),Times.Once)),
                    "23456"
                },
                //retrieved port is same as the old port some methods are not executed
                new object[]
                {
                    "12345",
                    new Action<Mock<IPvpnLogCopy>>((arg) => arg.Verify(el=> el.CopyLogsToProject(),Times.Once)),
                    new Action<Mock<IPortForwardingFinder>>((arg) => arg.Verify(el=> el.GetForwardedPort(),Times.Once)),
                    new Action<Mock<IQBitTorrentUserRetriever>>((arg) => arg.Verify(el=> el.GetQbitTorrentUserCredentials(),Times.Never)),
                    new Action<Mock<IQBitTorrentCommander>>((arg) => arg.Verify(el=> el.LoginToQBitTorrent(),Times.Never)),
                    new Action<Mock<IQBitTorrentCommander>,string>((arg,port) => arg.Verify(el=> el.SetForwardedPort(port),Times.Never)),
                    "12345"
                },

            };
        #endregion

    }
}
