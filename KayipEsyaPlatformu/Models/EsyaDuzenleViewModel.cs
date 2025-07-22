using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace KayipEsyaPlatformu.Models

{

    public class EsyaDuzenleViewModel
    {
        public int Id { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public int KategoriId { get; set; }
        public int DurumId { get; set; }
        public int RenkId { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string Konum { get; set; }
        public double? Enlem { get; set; }
        public double? Boylam { get; set; }
        public int SehirId { get; set; }
        public int IlceId { get; set; }
        public int MahalleId { get; set; }
        public string ResimYolu { get; set; }
        public IFormFile Resim { get; set; }
    }

}
