using System.ComponentModel.DataAnnotations;

namespace KayipEsyaPlatformu.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mevcut şifre gerekli")]
        [DataType(DataType.Password)]
        [Display(Name = "Mevcut Şifre")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yeni şifre gerekli")]
        [StringLength(100, ErrorMessage = "Şifre en az {2}, en fazla {1} karakter olmalı.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre (Tekrar)")]
        [Compare("NewPassword", ErrorMessage = "Yeni şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? StatusMessage { get; set; }
    }
}
