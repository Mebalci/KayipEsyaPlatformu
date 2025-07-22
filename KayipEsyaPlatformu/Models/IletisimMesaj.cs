using System;
using System.ComponentModel.DataAnnotations;

namespace KayipEsyaPlatformu.Models
{
    public class IletisimMesaj
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string AdSoyad { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Konu { get; set; }

        [Required]
        [StringLength(1000)]
        public string Mesaj { get; set; }

        public DateTime Tarih { get; set; } = DateTime.Now;
        public bool OkunduMu { get; set; } = false;
    }
} 