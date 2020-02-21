using QuakeLog.Domain.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO = QuakeLog.Domain.DTO;

namespace QuakeLog.Application
{
    public class GamesApplication : BaseApplication, IDisposable
    {
        private readonly IGameServices gameService;

        public GamesApplication(IGameServices gameService) : base()
            => this.gameService = gameService;

        public List<DTO.Games> GetAll()
        {
            var result = gameService.GetAll();

            if (result == null)
                throw new ArgumentException($"Não foi retornado nenhuma partida");

            return result;
        }

        public List<DTO.Games> GetWithoutWorld()
        {
            var result = gameService.GetWithoutWorld();

            if (result == null)
                throw new ArgumentException($"Não foi retornado nenhuma partida");

            return result;
        }

        public DTO.Games GetByGameId(int idGame)
        {
            var result = gameService.GetWithoutWorld(idGame)?.FirstOrDefault();

            if (result == null)
                throw new ArgumentException($"Game não encontrado");

            return result;
        }
    }
}
