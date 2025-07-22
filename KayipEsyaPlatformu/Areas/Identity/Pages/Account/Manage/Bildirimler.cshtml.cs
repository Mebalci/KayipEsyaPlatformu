using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using KayipEsyaPlatformu.Services;
using KayipEsyaPlatformu.Models;
using Microsoft.AspNetCore.Mvc;

namespace KayipEsyaPlatformu.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class BildirimlerModel : PageModel
    {
        private readonly BildirimServisi _bildirimServisi;

        public BildirimlerModel(BildirimServisi bildirimServisi)
        {
            _bildirimServisi = bildirimServisi;
        }

        public List<Bildirim> Bildirimler { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
            Bildirimler = await _bildirimServisi.Getir(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostOkunduYapAsync(int id)
        {
            await _bildirimServisi.OkunduYap(id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostTumunuOkunduYapAsync()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
            await _bildirimServisi.TumunuOkunduYap(userId);
            return RedirectToPage();
        }
    }
}
