using QuakeLog.Application;
using QuakeLog.Domain.Contracts.Services;
using QuakeLog.Tests.Mocks;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using DTO = QuakeLog.Domain.DTO;

namespace QuakeLog.Tests
{
    [ExcludeFromCodeCoverage]
    public class GamesApplicationTest
    {
        private Mock<GamesApplication> mockGamesApplication;

        public GamesApplicationTest()
        {
            var mockGameServices = new Mock<IGameServices>();
            mockGameServices
                .Setup(g => g.GetAll())
                .Returns(LogMocks.GetListGames());

            mockGameServices
                .Setup(g => g.GetWithoutWorld(1))
                .Returns(new List<DTO.Games>() { LogMocks.GetGame(1) });

            mockGameServices
                .Setup(g => g.GetWithoutWorld(null))
                .Returns(LogMocks.GetListGames());

            mockGamesApplication = new Mock<GamesApplication>(mockGameServices.Object);
        }

        #region "   GET GetAll  "

        [Fact]
        public void GET_GetAll_OK()
        {
            var loadLog = mockGamesApplication.Object.GetAll();
            Assert.True(loadLog.Count > 0);
        }

        [Fact]
        public void GET_GetAll_NOK()
        {
            var _mockGameServices = new Mock<IGameServices>();
            _mockGameServices
                .Setup(g => g.GetAll())
                .Returns((List<DTO.Games>)null);

            var _mockGamesApplication = new Mock<GamesApplication>(_mockGameServices.Object);

            var ex = Record.Exception(() => _mockGamesApplication.Object.GetAll());

            Assert.True(ex is ArgumentException);
        }

        #endregion

        #region "   GET GetWithoutWorld  "

        [Fact]
        public void GET_GetWithoutWorld_OK()
        {
            var loadLog = mockGamesApplication.Object.GetWithoutWorld();
            Assert.True(loadLog.Count > 0);
        }

        [Fact]
        public void GET_GetWithoutWorld_NOK()
        {
            var _mockGameServices = new Mock<IGameServices>();
            _mockGameServices
                .Setup(g => g.GetWithoutWorld(null))
                .Returns((List<DTO.Games>)null);

            var _mockGamesApplication = new Mock<GamesApplication>(_mockGameServices.Object);

            var ex = Record.Exception(() => _mockGamesApplication.Object.GetWithoutWorld());

            Assert.True(ex is ArgumentException);
        }

        #endregion

        #region "   GET GetByGameId  "

        [Fact]
        public void GET_GetByGameId_OK()
        {
            var id = 1;
            var loadLog = mockGamesApplication.Object.GetByGameId(id);
            Assert.True(loadLog.Id == id);
        }

        [Fact]
        public void GET_GetByGameId_NOK()
        {
            var _mockGameServices = new Mock<IGameServices>();
            _mockGameServices
                .Setup(g => g.GetWithoutWorld(It.IsAny<int>()))
                .Returns((List<DTO.Games>)null);

            var _mockGamesApplication = new Mock<GamesApplication>(_mockGameServices.Object);

            var ex = Record.Exception(() => _mockGamesApplication.Object.GetByGameId(1));

            Assert.True(ex is ArgumentException);
        }

        #endregion
    }
}
