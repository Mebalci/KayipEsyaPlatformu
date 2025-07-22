namespace KayipEsyaPlatformu.Models
{
    public class Sehir
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public ICollection<Ilce> Ilceler { get; set; }
    }
}
