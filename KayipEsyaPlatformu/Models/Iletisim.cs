using System.ComponentModel.DataAnnotations;

namespace KayipEsyaPlatformu.Models
{
    public class Iletisim
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Baslik { get; set; }
        
        [Required(ErrorMessage = "İçerik zorunludur.")]
        public string Icerik { get; set; }
        
        [Required(ErrorMessage = "Adres zorunludur.")]
        public string Adres { get; set; }
        
        [Required(ErrorMessage = "Telefon zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string Telefon { get; set; }
        
        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }
        
        public string? CalismaSaatleri { get; set; }
        public string? HaritaKoordinatlari { get; set; } // "lat,lng" formatında
        public string? SosyalMedyaLinkleri { get; set; } // JSON formatında
        public string? IletisimFormuMetni { get; set; }
        
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellemeTarihi { get; set; }
        public bool AktifMi { get; set; } = true;
    }
} 