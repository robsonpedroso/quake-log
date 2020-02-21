namespace QuakeLog.Domain.DTO
{
    public class KillMeans
    {
        public int Count { get; set; }
        public Enums.MeansOfDeath MeansOfDeath { get; set; }
        public string DescriptionMeansOfDeath { get; set; }

        public KillMeans(Enums.MeansOfDeath meansOfDeath, int count)
        {
            MeansOfDeath = meansOfDeath;
            DescriptionMeansOfDeath = meansOfDeath.ToString();
            Count = count;
        }
    }
}
