using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class EmailModel : PageModel
{
    private readonly UserManager<Kullanici> _userManager;
    private readonly IEmailSender _emailSender;

    public EmailModel(UserManager<Kullanici> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }

    [TempData]
    public string? StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Yeni E-posta")]
        public string NewEmail { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("Kullanýcý bulunamadý.");

        Email = await _userManager.GetEmailAsync(user) ?? "";
        Input.NewEmail = Email;
        IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("Kullanýcý bulunamadý.");

        var email = await _userManager.GetEmailAsync(user);
        if (Input.NewEmail != email)
        {
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
            var confirmationLink = Url.Page(
                "/Account/ConfirmEmailChange",
                null,
                new { userId = await _userManager.GetUserIdAsync(user), email = Input.NewEmail, code = token },
                Request.Scheme);

            await _emailSender.SendEmailAsync(Input.NewEmail, "E-posta Onayý",
                $"Lütfen yeni e-posta adresinizi onaylamak için <a href='{confirmationLink}'>buraya týklayýn</a>.");

            StatusMessage = "Doðrulama e-postasý gönderildi. Lütfen gelen kutunuzu kontrol edin.";
            return RedirectToPage();
        }

        StatusMessage = "E-posta adresiniz deðiþmedi.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostSendVerificationEmailAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("Kullanýcý bulunamadý.");

        var email = await _userManager.GetEmailAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var callbackUrl = Url.Page(
            "/Account/ConfirmEmail",
            null,
            new { userId = user.Id, code = code },
            Request.Scheme);

        await _emailSender.SendEmailAsync(email, "E-posta Doðrulama",
            $"Lütfen e-posta adresinizi doðrulamak için <a href='{callbackUrl}'>buraya týklayýn</a>.");

        StatusMessage = "Doðrulama e-postasý gönderildi.";
        return RedirectToPage();
    }
}
