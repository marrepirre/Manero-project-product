using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Product_Provider.Data.Entities;

public class ProductEntity
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string BatchNumber { get; set; } = null!;
	public string ProductName { get; set; } = null!;
	public string ProductDescription { get; set; } = null!;
	public string Color { get; set; } = null!;
	public string Size { get; set; } = null!;
	public string SmallImage { get; set; } = null!;
	public int Stock { get; set; }
	public bool IsBestSeller { get; set; }
    public bool IsNew { get; set; }
    public bool IsSale { get; set; }
    public bool IsTop { get; set; }
    public string CategoryId { get; set; } = null!;
	public CategoryEntity Category { get; set; } = null!;
	public string SubCategoryId { get; set; } = null!;
	public SubCategory Subcategory { get; set; } = null!;

	public decimal OriginalPrice { get; set; }
	public decimal? DiscountPrice { get; set; }
	public List<string> BigImage { get; set; } = [];

}
