using DictionaryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DictionaryApp.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Word> Words { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Word>()
                .HasKey(w => w.Id);

            modelBuilder.Entity<History>()
                .HasKey(h => h.Id);

            modelBuilder.Entity<History>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Configura o relacionamento com o usuário
        }

    }
}
