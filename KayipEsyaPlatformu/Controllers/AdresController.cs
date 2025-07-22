using KayipEsyaPlatformu.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("Adres")]
public class AdresController : Controller
{
    private readonly ApplicationDbContext _db;
    public AdresController(ApplicationDbContext db) => _db = db;

    [HttpGet("Ilceler/{sehirId}")]
    public async Task<IActionResult> Ilceler(int sehirId)
    {
        var data = await _db.Ilceler
            .Where(i => i.SehirId == sehirId)
            .Select(i => new { i.Id, i.Ad })
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("Mahalleler/{ilceId}")]
    public async Task<IActionResult> Mahalleler(int ilceId)
    {
        var data = await _db.Mahalleler
            .Where(m => m.IlceId == ilceId)
            .Select(m => new { m.Id, m.Ad })
            .ToListAsync();
        return Ok(data);
    }
}

