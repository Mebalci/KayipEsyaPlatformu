namespace KayipEsyaPlatformu.Services
{
    public class TopluEslesmeZamanlayici : BackgroundService
    {
        private readonly IServiceProvider _provider;

        public TopluEslesmeZamanlayici(IServiceProvider provider)
        {
            _provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _provider.CreateScope();
                    var servis = scope.ServiceProvider.GetRequiredService<IEsyaEslesmeServisi>();
                    await servis.TümEsyalarIcinTopluEslesmeAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Zamanlayıcı Hatası] {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
