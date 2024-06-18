using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Api.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Image> Images => Set<Image>();
}