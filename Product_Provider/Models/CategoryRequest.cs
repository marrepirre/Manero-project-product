namespace Product_Provider.Models;

public class CategoryRequest
{
	public string CategoryName { get; set; } = null!;
	public List<string>? SubCategories { get; set; }
}
