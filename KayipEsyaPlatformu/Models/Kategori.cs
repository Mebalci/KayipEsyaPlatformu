namespace KayipEsyaPlatformu.Models
{
    public class Kategori
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;

        public ICollection<Esya> Esyalar { get; set; }
    }

}
