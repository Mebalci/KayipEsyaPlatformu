using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

public class PersonalDataModel : PageModel
{
    private readonly UserManager<Kullanici> _userManager;

    public PersonalDataModel(UserManager<Kullanici> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> OnPostDownloadAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("Kullanýcý bulunamadý.");

        var personalData = new Dictionary<string, string?>
        {
            { "Kullanýcý ID", user.Id },
            { "Email", user.Email },
            { "Ad", user.Ad },
            { "Soyad", user.Soyad },
            { "Telefon Numarasý", user.PhoneNumber }
        };

        var json = JsonSerializer.Serialize(personalData, new JsonSerializerOptions { WriteIndented = true });
        var bytes = Encoding.UTF8.GetBytes(json);

        return File(bytes, "application/json", "personal-data.json");
    }

    public void OnGet()
    {
    }
}
