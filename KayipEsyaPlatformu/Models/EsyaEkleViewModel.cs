using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace KayipEsyaPlatformu.Models

{
    public class EsyaEkleViewModel
    {
        [Required]
        public string Baslik { get; set; }

        [Required]
        public string Aciklama { get; set; }

        public string Marka { get; set; }
        public string Model { get; set; }

        public double? Enlem { get; set; }
        public double? Boylam { get; set; }

        [Required]
        public string Konum { get; set; }

        [Required]
        public int DurumId { get; set; }

        [Required]
        public int KategoriId { get; set; }

        [Required]
        public int SehirId { get; set; }

        [Required]
        public int IlceId { get; set; }

        [Required]
        public int MahalleId { get; set; }

        [Required]
        public int RenkId { get; set; }

        [Required]
        public IFormFile Resim { get; set; }

        public DateTime Tarih { get; set; }
        public bool SahipUlasildiMi { get; set; } = false;

        public string KullaniciId { get; set; }
    }
}
