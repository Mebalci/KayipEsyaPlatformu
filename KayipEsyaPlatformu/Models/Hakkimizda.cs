using System.ComponentModel.DataAnnotations;

namespace KayipEsyaPlatformu.Models
{
    public class Hakkimizda
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Baslik { get; set; }
        
        [Required(ErrorMessage = "İçerik zorunludur.")]
        public string Icerik { get; set; }
        
        public string? Misyon { get; set; }
        public string? Vizyon { get; set; }
        public string? Degerlerimiz { get; set; }
        public string? Ekibimiz { get; set; }
        public string? Tarihce { get; set; }
        
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellemeTarihi { get; set; }
        public bool AktifMi { get; set; } = true;
    }
} 