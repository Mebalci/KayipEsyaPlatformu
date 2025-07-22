using KayipEsyaPlatformu.Models;

namespace KayipEsyaPlatformu.Services
{
    public interface IEslesmeQueue
    {
        void Enqueue(Esya esya);
        Task<Esya> DequeueAsync(CancellationToken token);
    }
}
