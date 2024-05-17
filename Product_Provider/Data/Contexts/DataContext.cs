using Microsoft.EntityFrameworkCore;
using Product_Provider.Data.Entities;

namespace Product_Provider.Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
	public DbSet<ProductEntity> Products { get; set; }
	public DbSet<CategoryEntity> Categories { get; set; }
	public DbSet<SubCategory> SubCategories { get; set; }
	public DbSet<ColorEntity> Colors { get; set; }
	public DbSet<SizeEntity> Sizes { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ProductEntity>().ToContainer("Products").HasPartitionKey(x => x.BatchNumber);
		modelBuilder.Entity<CategoryEntity>().ToContainer("Categories").HasPartitionKey(x => x.CategoryName);
		modelBuilder.Entity<SubCategory>().ToContainer("SubCategories").HasPartitionKey(x => x.SubCategoryName);
		modelBuilder.Entity<ColorEntity>().ToContainer("Colors").HasPartitionKey(x => x.Color);
		modelBuilder.Entity<SizeEntity>().ToContainer("Sizes").HasPartitionKey(x => x.Size);
	}
}
