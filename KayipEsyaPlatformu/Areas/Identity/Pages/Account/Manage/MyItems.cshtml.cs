using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using KayipEsyaPlatformu.Models;
using KayipEsyaPlatformu.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace KayipEsyaPlatformu.Areas.Identity.Pages.Account.Manage
{
    public class MyItemsModel : PageModel
    {
        private readonly UserManager<Kullanici> _userManager;
        private readonly ApplicationDbContext _context;

        public MyItemsModel(UserManager<Kullanici> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<Esya> Esyalar { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            Esyalar = await _context.Esyalar
                .Include(e => e.Durum)
                .Where(e => e.KullaniciId == user.Id)
                .OrderByDescending(e => e.Tarih)
                .ToListAsync();

            return Page();
        }
    }
}
