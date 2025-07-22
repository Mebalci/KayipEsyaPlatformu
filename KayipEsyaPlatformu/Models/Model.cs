namespace KayipEsyaPlatformu.Models
{
    public class Model
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public int MarkaId { get; set; }
        public Marka Marka { get; set; }
    }
}
