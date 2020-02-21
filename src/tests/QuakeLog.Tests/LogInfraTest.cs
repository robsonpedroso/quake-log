using QuakeLog.Domain;
using QuakeLog.Infra.Services;
using QuakeLog.Tests.Mocks;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace QuakeLog.Tests
{
    [ExcludeFromCodeCoverage]
    public class LogInfraTest
    {
        private Mock<LogProcessInfraServices> mockLogProcessInfraServices;

        public LogInfraTest()
        {
            var configuration = new Mock<IConfiguration>();

            var config = new Mock<Config>(configuration.Object);
            config.Setup(c => c.AllActions).Returns("(InitGame|ClientConnect|ClientUserinfoChanged|Kill)");
            config.Setup(c => c.LogPathName).Returns("C:\\test");
            config.Setup(c => c.World).Returns(1024);

            mockLogProcessInfraServices = new Mock<LogProcessInfraServices>(config.Object);
            mockLogProcessInfraServices
                .Setup(c => c.ReaderFile("C:\\test"))
                .Returns(LogMocks.LinesFile);
        }

        [Fact]
        public void LoadFile()
        {
            var loadLog = mockLogProcessInfraServices.Object.Load();
            Assert.True(loadLog.Count > 0);
        }
    }
}
