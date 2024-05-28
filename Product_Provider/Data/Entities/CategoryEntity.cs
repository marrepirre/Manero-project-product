namespace Product_Provider.Data.Entities;

public class CategoryEntity
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string? CategoryName { get; set; }
	
}

