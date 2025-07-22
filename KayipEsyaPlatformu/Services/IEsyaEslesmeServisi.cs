using KayipEsyaPlatformu.Models;

public interface IEsyaEslesmeServisi
{
    Task EsyaIcinEslesmeleriHesaplaAsync(Esya esya);
    Task TümEsyalarIcinTopluEslesmeAsync();
}
