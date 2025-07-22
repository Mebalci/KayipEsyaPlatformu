using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


    public class Kullanici : IdentityUser
    {
        [Required]
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public bool AktifMi { get; set; } = true;
    }




