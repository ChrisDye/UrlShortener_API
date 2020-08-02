using Entities.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.ValueGeneration.Internal;

namespace Entities.Context
{
    public class UrlContext : DbContext
    {
        public DbSet<Url> Urls { get; set; }

        public UrlContext(DbContextOptions<UrlContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Url>(entity =>
            {
                entity.HasKey(e => e.Id);
                var id = entity.Property(p => p.Id);
                id.ValueGeneratedOnAdd();

                var created = entity.Property(p => p.Created);
                created.ValueGeneratedOnAdd();
                //if (Database.IsInMemory)
                //{
                //    id.HasValueGenerator<InMemoryIntegerValueGenerator<int>>();
                //}
            });
        }
    }
}
