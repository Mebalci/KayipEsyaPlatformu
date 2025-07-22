using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    private readonly UserManager<Kullanici> _userManager;
    private readonly SignInManager<Kullanici> _signInManager;

    public IndexModel(UserManager<Kullanici> userManager, SignInManager<Kullanici> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [TempData]
    public string? StatusMessage { get; set; }

    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required]
        [Display(Name = "Ad")]
        public string Ad { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("Kullanýcý bulunamadý.");

        Username = user.UserName!;
        Input = new InputModel
        {
            Ad = user.Ad,
            Soyad = user.Soyad,
            PhoneNumber = user.PhoneNumber ?? ""
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("Kullanýcý bulunamadý.");

        user.Ad = Input.Ad;
        user.Soyad = Input.Soyad;
        user.PhoneNumber = Input.PhoneNumber;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            foreach (var error in updateResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Profil baþarýyla güncellendi.";

        return RedirectToPage();
    }
}
