using Microsoft.EntityFrameworkCore;
namespace SushiStore.Models 
{
 public class Sushi
 {
 public int Id { get; set; }
 public string? Name { get; set; }
 public string? Description { get; set; }
 }
 class SushiDb : DbContext
{
 public SushiDb(DbContextOptions options) : base(options) { }
 public DbSet<Sushi> Sushis { get; set; } = null!;
}
}
