using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class ChangePasswordModel : PageModel
{
    private readonly UserManager<Kullanici> _userManager;
    private readonly SignInManager<Kullanici> _signInManager;

    public ChangePasswordModel(UserManager<Kullanici> userManager, SignInManager<Kullanici> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? StatusMessage { get; set; }

    public class InputModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mevcut �ifre")]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "�ifre en az {2}, en fazla {1} karakter olmal�.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni �ifre")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Yeni �ifre (Tekrar)")]
        [Compare("NewPassword", ErrorMessage = "Yeni �ifreler e�le�miyor.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("Kullan�c� bulunamad�.");

        var hasPassword = await _userManager.HasPasswordAsync(user);
        if (!hasPassword)
            return RedirectToPage("./SetPassword");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("Kullan�c� bulunamad�.");

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            foreach (var error in changePasswordResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return Page();
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "�ifreniz ba�ar�yla de�i�tirildi.";

        return RedirectToPage();
    }
}
