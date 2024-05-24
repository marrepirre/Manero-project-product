using Microsoft.EntityFrameworkCore;
using Product_Provider.Data.Entities;
using System.Security.Permissions;


namespace Product_Provider.Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ColorEntity> Colors { get; set; }
    public DbSet<SizeEntity> Sizes { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<ProductEntity> Products { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseLazyLoadingProxies();
	}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
		modelBuilder.Entity<ColorEntity>().ToContainer("Colors").HasPartitionKey(x => x.Color);
        modelBuilder.Entity<SizeEntity>().ToContainer("Sizes").HasPartitionKey(x => x.Size);
        modelBuilder.Entity<CategoryEntity>().ToContainer("Categories").HasPartitionKey(x => x.CategoryName);
        modelBuilder.Entity<ProductEntity>().ToContainer("Products").HasPartitionKey(x => x.BatchNumber);

    }
}
