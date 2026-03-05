
using FluentAssertions;
using Moq;
using Moq.Protected;
using QBitTorrentPortForwardSetterViaPVPN.Constants;
using QBitTorrentPortForwardSetterViaPVPN.Helpers;
using QBitTorrentPortForwardSetterViaPVPN.Models;
using QBitTorrentPortForwardSetterViaPVPN.Services;
using System.Net;

namespace QBitTorrentPortForwardSetterViaPVPNTests.Services
{
    public class QBitTorrentCommanderTests
    {
        private QBitTorrentCommander qBitTorrentCommander;

        Mock<IQBitTorrentUserRetriever> qBitTorrentUserRetriever = new Mock<IQBitTorrentUserRetriever>();

        private PortForwardingFinder portForwardingFinder;

        private  PathConstants pathConstants;

        private  LogsHelper logHelper;

        private HttpClient httpClient;

        private void InitPortForwardingFinder() 
        {
            this.pathConstants = new PathConstants();

            this.logHelper = new LogsHelper();

            this.portForwardingFinder = new PortForwardingFinder(this.pathConstants, this.logHelper);
        }

        private void MockQbitTorrentUserRetriever(string username, string password) 
        {
            QbitTorrentUserModel userModel = new QbitTorrentUserModel() { Username = username, Password = password };

            this.qBitTorrentUserRetriever.Setup(el => el.GetQbitTorrentUserCredentials()).Returns(userModel);
        }

        public QBitTorrentCommanderTests()
        {
            this.InitPortForwardingFinder();

            this.MockQbitTorrentUserRetriever("testUsername", "TestPassword");

            this.qBitTorrentCommander = new QBitTorrentCommander(qBitTorrentUserRetriever.Object, portForwardingFinder!,null);
        }

        [Fact]
        public async Task LoginToQBitTorrent_PrintsSuccessMessage()
        {
            this.MockQbitTorrentUserRetriever("test","pass");

            Mock<PortForwardingFinder> portForwardingFinderMock = new Mock<PortForwardingFinder>();

            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            this.httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(QBitTorrentConstants.BaseAddress)
            };

            QBitTorrentCommander qBitTorrentCommander = new QBitTorrentCommander(
                this.qBitTorrentUserRetriever.Object,
                this.portForwardingFinder,
                this.httpClient);

            string output = CaptureConsoleOutput(async () => await qBitTorrentCommander.LoginToQBitTorrent());

            output.Should().Contain("Log in to qBittorrent Succesfull");
        }

        [Fact]
        public async Task SetForwardedPort_PrintsSuccessMessage()
        {
            Mock<PortForwardingFinder> portForwardingFinderMock = new Mock<PortForwardingFinder>();

            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            this.httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(QBitTorrentConstants.BaseAddress)
            };

            QBitTorrentCommander qBitTorrentCommander = new QBitTorrentCommander(
                this.qBitTorrentUserRetriever.Object,
                this.portForwardingFinder,
                this.httpClient);

            string output = await CaptureConsoleOutputAsync(
                            async (port) => await qBitTorrentCommander.SetForwardedPort(port),
                            "12345");

            output.Should().Contain("Port set in qBittorrent!");
        }

        private static string CaptureConsoleOutput(Action action)
        {
            var stringWriter = new StringWriter();

            var originalOutput = Console.Out;

            Console.SetOut(stringWriter);

            try
            {
                action();
                return stringWriter.ToString();
            }
            finally
            {
                Console.SetOut(originalOutput);
            }
        }

        private static async Task<string> CaptureConsoleOutputAsync(Func<string, Task> asyncAction, string parameter)
        {
            var stringWriter = new StringWriter();

            var originalOutput = Console.Out;

            Console.SetOut(stringWriter);

            try
            {
                await asyncAction(parameter);

                return stringWriter.ToString();
            }
            finally
            {
                Console.SetOut(originalOutput);
            }
        }

    }
}
