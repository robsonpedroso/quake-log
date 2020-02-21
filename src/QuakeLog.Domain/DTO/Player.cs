namespace QuakeLog.Domain.DTO
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }

        public Player(int id, string name = "")
        {
            this.Id = id;
            this.Name = name;
            this.Rank = 0;
        }

        public void SetRank(bool up = true)
        {
            if (up)
                this.Rank++;
            else
            {
                if (this.Rank > 0)
                    this.Rank--;
            }
        }
    }
}
