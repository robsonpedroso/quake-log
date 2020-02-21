using QuakeLog.Application;
using QuakeLog.Domain;
using QuakeLog.Domain.Contracts.InfraServices;
using QuakeLog.Domain.Contracts.Services;
using QuakeLog.Domain.Services;
using QuakeLog.Tests.Mocks;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using DTO = QuakeLog.Domain.DTO;

namespace QuakeLog.Tests
{
    [ExcludeFromCodeCoverage]
    public class GameServicesTest
    {
        private Mock<GameServices> mockGameServices;
        private Mock<Config> config;

        public GameServicesTest()
        {
            var configuration = new Mock<IConfiguration>();

            config = new Mock<Config>(configuration.Object);
            config.Setup(c => c.AllActions).Returns("(InitGame|ClientConnect|ClientUserinfoChanged|Kill)");
            config.Setup(c => c.LogPathName).Returns("C:\\test");
            config.Setup(c => c.World).Returns(1024);

            var mockLogInfraServices = new Mock<ILogProcessInfraServices>();
            mockLogInfraServices
                .Setup(g => g.Load())
                .Returns(LogMocks.GetListGames());

            mockGameServices = new Mock<GameServices>(mockLogInfraServices.Object, config.Object);
        }

        #region "   GET GetAll  "

        [Fact]
        public void GET_GetAll_OK()
        {
            var loadLog = mockGameServices.Object.GetAll();
            Assert.True(loadLog.Count > 0);
        }

        [Fact]
        public void GET_GetAll_NOK()
        {
            var _mockLogInfraServices = new Mock<ILogProcessInfraServices>();
            _mockLogInfraServices
                .Setup(g => g.Load())
                .Returns((List<DTO.Games>)null);

            var _mockGameServices = new Mock<GameServices>(_mockLogInfraServices.Object, config.Object);

            var result = _mockGameServices.Object.GetAll();

            Assert.Null(result);
        }

        #endregion


        #region "   GET GetWithoutWorld  "

        [Fact]
        public void GET_GetWithoutWorld_OK()
        {
            var loadLog = mockGameServices.Object.GetWithoutWorld();
            Assert.True(loadLog.Count > 0);
        }

        [Fact]
        public void GET_GetWithoutWorld_NOK()
        {
            var _mockLogInfraServices = new Mock<ILogProcessInfraServices>();
            _mockLogInfraServices
                .Setup(g => g.Load())
                .Returns((List<DTO.Games>)null);

            var _mockGameServices = new Mock<GameServices>(_mockLogInfraServices.Object, config.Object);

            var result = _mockGameServices.Object.GetWithoutWorld();

            Assert.Null(result);
        }

        [Fact]
        public void GET_GetWithoutWorld_Param_OK()
        {
            var loadLog = mockGameServices.Object.GetWithoutWorld(1);
            Assert.True(loadLog.Count > 0);
        }

        [Fact]
        public void GET_GetWithoutWorld_Param_NOK()
        {
            var _mockLogInfraServices = new Mock<ILogProcessInfraServices>();
            _mockLogInfraServices
                .Setup(g => g.Load())
                .Returns((List<DTO.Games>)null);

            var _mockGameServices = new Mock<GameServices>(_mockLogInfraServices.Object, config.Object);

            var result = _mockGameServices.Object.GetWithoutWorld(1);

            Assert.Null(result);
        }

        [Fact]
        public void GET_GetWithoutWorld_Param_NotFound_NOK()
        {
            var result = mockGameServices.Object.GetWithoutWorld(100);

            Assert.True(result.Count == 0);
        }

        #endregion
    }
}
