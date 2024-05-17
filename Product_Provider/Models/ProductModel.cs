namespace Product_Provider.Models;

public class ProductModel
{
	public string BatchNumber { get; set; } = null!;
	public string ProductName { get; set; } = null!;
	public string ProductDescription { get; set; } = null!;
	public string Color { get; set; } = null!;
	public string Size { get; set; } = null!;
	public string SmallImage { get; set; } = null!;
	public int Stock { get; set; }
	public bool IsBestSeller { get; set; }
	public string SubCategoryName { get; set; } = null!;
	public decimal OriginalPrice { get; set; }
	public decimal? DiscountPrice { get; set; }
	public List<string> BigImage { get; set; } = [];
}
