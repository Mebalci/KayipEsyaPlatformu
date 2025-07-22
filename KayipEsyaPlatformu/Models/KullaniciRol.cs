using System.ComponentModel.DataAnnotations;

namespace KayipEsyaPlatformu.Models
{
    public class KullaniciRol
    {
        public int Id { get; set; }

        [Required]
        public string KullaniciId { get; set; } = string.Empty;
        public int RolId { get; set; }

        public Kullanici? Kullanici { get; set; }
        
    }

}
