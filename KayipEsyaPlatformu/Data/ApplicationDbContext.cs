using KayipEsyaPlatformu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KayipEsyaPlatformu.Data
{
    public class ApplicationDbContext : IdentityDbContext<Kullanici>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Esya> Esyalar { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<EsyaDurumu> EsyaDurumlari { get; set; }
        public DbSet<Eslesme> Eslesmeler { get; set; }
        public DbSet<Mesaj> Mesajlar { get; set; }
        public DbSet<Bildirim> Bildirimler { get; set; }
        public DbSet<TeslimNoktasi> TeslimNoktalari { get; set; }
        public DbSet<SahteBildirim> SahteBildiriler { get; set; }
        public DbSet<Sehir> Sehirler { get; set; }
        public DbSet<Ilce> Ilceler { get; set; }
        public DbSet<Mahalle> Mahalleler { get; set; }
        public DbSet<Marka> Markalar { get; set; }
        public DbSet<Model> Modeller { get; set; }
        public DbSet<Renk> Renkler { get; set; }
        public DbSet<Hakkimizda> Hakkimizda { get; set; }
        public DbSet<Iletisim> Iletisim { get; set; }
        public DbSet<IletisimMesaj> IletisimMesajlari { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>(b =>
            {
                b.Property(u => u.Id).HasMaxLength(255);
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.Property(r => r.Id).HasMaxLength(255);
            });
        }

    }
}
