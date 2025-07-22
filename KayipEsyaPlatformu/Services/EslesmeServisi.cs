using KayipEsyaPlatformu.Models;

public class EslesmeServisi
{
    private readonly HttpClient _httpClient;

    public EslesmeServisi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EsyaEslesmeSonucu>> EslestirAsync(Esya input, List<Esya> adaylar)
    {
        var payload = new
        {
            input = new
            {
                eşya = input.Baslik,
                marka = input.Marka,
                model = input.Model,
                renk = input.Renk?.Ad,
                şehir = input.Sehir?.Ad,
                ilçe = input.Ilce?.Ad,
                mahalle = input.Mahalle?.Ad,
                açıklama = input.Aciklama,
                konum = input.Konum,
                enlem = input.Enlem,
                boylam = input.Boylam
            },
            aday = adaylar.Select(a => new {
                id = a.Id,
                eşya = a.Baslik,
                marka = a.Marka,
                model = a.Model,
                renk = a.Renk?.Ad,
                şehir = a.Sehir?.Ad,
                ilçe = a.Ilce?.Ad,
                mahalle = a.Mahalle?.Ad,
                açıklama = a.Aciklama,
                konum = a.Konum,
                enlem = a.Enlem,
                boylam = a.Boylam
            }).ToList()

        };

        var response = await _httpClient.PostAsJsonAsync("http://localhost:5000/match", payload);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<EsyaEslesmeSonucu>>() ?? new();
    }
}
