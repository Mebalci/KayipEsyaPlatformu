using KayipEsyaPlatformu.Models;
using System.Collections.Concurrent;

namespace KayipEsyaPlatformu.Services
{
    public class EslesmeQueue : IEslesmeQueue
    {
        private readonly ConcurrentQueue<Esya> _queue = new();
        private readonly SemaphoreSlim _signal = new(0);

        public void Enqueue(Esya esya)
        {
            _queue.Enqueue(esya);
            _signal.Release();
        }

        public async Task<Esya> DequeueAsync(CancellationToken token)
        {
            await _signal.WaitAsync(token);
            _queue.TryDequeue(out var esya);
            return esya!;
        }
    }

}
