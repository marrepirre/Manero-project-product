using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Product_Provider.Data.Entities;

public class ColorEntity
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Color { get; set; } = null!;
}
