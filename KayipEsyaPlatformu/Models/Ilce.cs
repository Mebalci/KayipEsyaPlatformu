namespace KayipEsyaPlatformu.Models
{
    public class Ilce
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public int SehirId { get; set; }
        public Sehir Sehir { get; set; }
        public ICollection<Mahalle> Mahalleler { get; set; }
    }
}
