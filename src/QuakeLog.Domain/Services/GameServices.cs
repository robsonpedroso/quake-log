using QuakeLog.Domain.Contracts.InfraServices;
using QuakeLog.Domain.Contracts.Services;
using System.Collections.Generic;
using System.Linq;

namespace QuakeLog.Domain.Services
{
    public class GameServices : IGameServices
    {
        private readonly ILogProcessInfraServices logService;
        private readonly Config config;

        public GameServices(ILogProcessInfraServices logService, Config config) : base()
        {
            this.logService = logService;
            this.config = config;
        }

        public List<DTO.Games> GetAll()
        {
            var result = logService.Load();

            if (result == null) return null;

            SetKillsMeans(result, false);

            return result;
        }

        public List<DTO.Games> GetWithoutWorld(int? idGame = null)
        {
            var result = logService.Load();

            if (result == null) return null;

            if (idGame.HasValue)
                result = result.Where(g => g.Id == idGame.Value)?.ToList();

            SetKillsMeans(result);

            return result;
        }

        private void SetKillsMeans(List<DTO.Games> result, bool removeWorld = true)
        {
            foreach (var game in result)
            {
                game.SetKillMeans();

                if (removeWorld)
                    game.Kills.RemoveAll(x => x.Killer.Id == config.World);
            }
        }
    }
}
