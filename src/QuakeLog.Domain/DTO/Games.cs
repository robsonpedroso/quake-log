using System.Collections.Generic;
using System.Linq;

namespace QuakeLog.Domain.DTO
{
    public class Games
    {
        public int Id { get; set; }
        public int TotalKills { get; set; }
        public List<Player> Players { get; set; }
        public List<KillPlayer> Kills { get; set; }
        public List<KillMeans> KillsMeans { get; set; }

        public Games(int id)
        {
            this.Id = id;
            this.Players = new List<Player>();
            this.Kills = new List<KillPlayer>();
            this.KillsMeans = new List<KillMeans>();
        }

        public void SetKillMeans()
        {
            var group = Kills.GroupBy(kills => kills.MeansOfDeath);

            foreach (var kills in group)
            {
                KillsMeans.Add(new KillMeans(kills.Key, kills.Count()));
            }
        }
    }
}
