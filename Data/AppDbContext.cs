using System.Text.Json;
using JobiJoba;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext{
    public DbSet<Ad> Ads{get; set;}

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var options = new JsonSerializerOptions{};
        
        modelBuilder.Entity<Ad>()
            .Property(ad => ad.ContractTypes)
            .HasConversion(
                v => JsonSerializer.Serialize(v, options),
                v => JsonSerializer.Deserialize<List<string>>(v, options) ?? new List<string>()
            );

        base.OnModelCreating(modelBuilder);
    }
}