namespace KayipEsyaPlatformu.Services
{
    public class EsyaEslesmeWorker : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly IEslesmeQueue _queue;

        public EsyaEslesmeWorker(IServiceProvider provider, IEslesmeQueue queue)
        {
            _provider = provider;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var esya = await _queue.DequeueAsync(stoppingToken);
                using var scope = _provider.CreateScope();
                var servisi = scope.ServiceProvider.GetRequiredService<IEsyaEslesmeServisi>();
                await servisi.EsyaIcinEslesmeleriHesaplaAsync(esya);
            }
        }
    }

}
