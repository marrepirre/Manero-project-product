namespace Product_Provider.Models;

public class ProductRequest
{
    public string BatchNumber { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string ProductDescription { get; set; } = null!;
    public string Color { get; set; } = null!;
    public string Size { get; set; } = null!;
    public int Stock { get; set; }
    public bool IsBestSeller { get; set; }
    public bool IsNew { get; set; }
    public bool IsSale { get; set; }
    public bool IsTop { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string? Category { get; set; }
    public string? ThumbnailImage { get; set; }
    public List<string> Images { get; set; } = [];
}
