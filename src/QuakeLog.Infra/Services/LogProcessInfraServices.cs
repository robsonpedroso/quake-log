using QuakeLog.Domain;
using QuakeLog.Domain.Contracts.InfraServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DTO = QuakeLog.Domain.DTO;

namespace QuakeLog.Infra.Services
{
    public class LogProcessInfraServices : ILogProcessInfraServices
    {
        private readonly Config config;

        public LogProcessInfraServices(Config config) => this.config = config;

        // is separate for easier Unit testing
        public virtual IEnumerable<string> ReaderFile(string pathFull)
        {
            if (!File.Exists(pathFull))
                throw new ArgumentException($"Arquivo de log não encontrado na pasta { pathFull }");

            return File.ReadLines(pathFull, Encoding.UTF8);
        }

        public List<DTO.Games> Load()
        {
            List<DTO.Games> games = new List<DTO.Games>();
            DTO.Games current = null;
            int countGames = 0;

            var actionsGames = new Regex(config.AllActions);

            foreach (string line in ReaderFile(config.LogPathName))
            {
                Match action = actionsGames.Match(line);

                switch (action.Value)
                {
                    case "InitGame":
                        current = InitGame(games, ++countGames);
                        break;
                    case "ClientConnect":
                        ConnectPlayer(current, line);
                        break;
                    case "ClientUserinfoChanged":
                        ChangedPlayer(current, line);
                        break;
                    case "Kill":
                        KillPlayer(current, line);
                        break;
                    default:
                        break;
                }
            }

            return games;
        }

        private DTO.Games InitGame(List<DTO.Games> games, int countGames)
        {
            var newGame = new DTO.Games(countGames);
            games.Add(newGame);

            return newGame;
        }

        private void ConnectPlayer(DTO.Games current, string line)
        {
            var find = " ClientConnect: ";

            var id = Convert.ToInt32(line.Substring(line.IndexOf(find) + find.Length));

            var player = current.Players.FirstOrDefault(x => x.Id == id);

            if (player == null)
                current.Players.Add(new DTO.Player(id));
        }

        private void ChangedPlayer(DTO.Games current, string line)
        {
            var find = " ClientUserinfoChanged: ";

            var content = line.Substring(line.IndexOf(find) + find.Length);
            var id = Convert.ToInt32(content.Substring(0, content.IndexOf(@"n\")));

            content = line.Substring(line.IndexOf(@"n\") + 2);
            var name = content.Substring(0, content.IndexOf(@"\t\"));

            var player = current.Players.FirstOrDefault(x => x.Id == id);

            if (player == null)
                current.Players.Add(new DTO.Player(id, name));
            else
                player.Name = name;
        }

        private void KillPlayer(DTO.Games current, string line)
        {
            var find = " Kill: ";

            var content = line.Substring(line.IndexOf(find) + find.Length);
            content = content.Substring(0, content.IndexOf(": "));

            string[] infoPlayers = content.Split(' ');
            var idKiller = Convert.ToInt32(infoPlayers[0]);
            var idKilled = Convert.ToInt32(infoPlayers[1]);
            var meansOfDeath = Convert.ToInt32(infoPlayers[2]);

            var playerKiller = current.Players.FirstOrDefault(x => x.Id == idKiller) ?? new DTO.Player(idKiller, idKiller == config.World ? "World" : string.Empty);
            var playerKilled = current.Players.FirstOrDefault(x => x.Id == idKilled);

            if (playerKiller.Id == config.World)
                playerKilled.SetRank(false);
            else
                playerKiller.SetRank();

            var killPlayer = new DTO.KillPlayer(playerKiller, playerKilled, (Domain.Enums.MeansOfDeath)meansOfDeath);

            current.Kills.Add(killPlayer);
            current.TotalKills++;
        }
    }
}
