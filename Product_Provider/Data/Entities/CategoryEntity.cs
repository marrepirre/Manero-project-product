using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Product_Provider.Data.Entities;

public class CategoryEntity
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string CategoryName { get; set; } = null!;

}

public class SubCategory
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string SubCategoryName { get; set; } = null!;

	public string CategoryId { get; set; } = null!;
	public CategoryEntity Category { get; set; } = null!;
}
