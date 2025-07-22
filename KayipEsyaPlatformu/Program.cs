using KayipEsyaPlatformu.Data;
using KayipEsyaPlatformu.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using KayipEsyaPlatformu.Areas.Identity.Data;


namespace KayipEsyaPlatformu
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");          

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 42)) 
            ));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<Kullanici>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            });

            var baseUrl = builder.Configuration["EslestirmeServisi:BaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("EslestirmeServisi:BaseUrl appsettings.json'da tanımlı olmalı!");
            builder.Services.AddHttpClient<EslesmeServisi>(client => {
                client.BaseAddress = new Uri(baseUrl);
            });

            builder.Services.AddScoped<IEsyaEslesmeServisi, EsyaEslesmeServisi>();

            builder.Services.AddScoped<IletisimMesajServisi>();

            builder.Services.AddSingleton<IEslesmeQueue, EslesmeQueue>();
            builder.Services.AddHostedService<EsyaEslesmeWorker>();

            builder.Services.AddHostedService<TopluEslesmeZamanlayici>();

            builder.Services.AddSignalR();

            
            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            
            builder.Services.AddScoped<BildirimServisi>();


            builder.Services.AddAuthentication().AddCookie();
            builder.Services.AddAuthorization();
            builder.Services.AddSignalR().AddHubOptions<BildirimHub>(options =>
            {
                options.EnableDetailedErrors = true;
            });



            var app = builder.Build();

            app.MapHub<BildirimHub>("/bildirimHub");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // Roller otomatik oluşturulsun
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roller = new[] { "Kullanici", "Admin" };
                foreach (var rol in roller)
                {
                    if (!roleManager.RoleExistsAsync(rol).Result)
                    {
                        roleManager.CreateAsync(new IdentityRole(rol)).Wait();
                    }
                }
            }

            app.Run();
        }
    }
}
