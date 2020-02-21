namespace QuakeLog.Domain.DTO
{
    public class KillPlayer
    {
        public Player Killer { get; set; }
        public Player Killed { get; set; }
        public Enums.MeansOfDeath MeansOfDeath { get; set; }
        public string DescriptionMeansOfDeath { get; set; }

        public KillPlayer(Player killer, Player killed, Enums.MeansOfDeath meansOfDeath)
        {
            this.Killer = killer;
            this.Killed = killed;
            this.MeansOfDeath = meansOfDeath;
            this.DescriptionMeansOfDeath = meansOfDeath.ToString();
        }
    }
}
