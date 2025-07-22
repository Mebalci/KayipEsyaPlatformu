using KayipEsyaPlatformu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class BildirimController : Controller
{
    private readonly BildirimServisi _bildirimServisi;

    public BildirimController(BildirimServisi bildirimServisi)
    {
        _bildirimServisi = bildirimServisi;
    }

    public async Task<IActionResult> Index()
    {
        var kullaniciId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var bildirimler = await _bildirimServisi.Getir(kullaniciId!);
        return View(bildirimler);
    }
    [HttpPost]
    public async Task<IActionResult> OkunduYap(int id)
    {
        await _bildirimServisi.OkunduYap(id);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> TumunuOkunduYap()
    {
        var kullaniciId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
        await _bildirimServisi.TumunuOkunduYap(kullaniciId);
        return Ok();
    }
}
