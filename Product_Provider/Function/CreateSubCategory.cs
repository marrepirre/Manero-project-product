using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Product_Provider.Data.Contexts;
using Product_Provider.Data.Entities;
using Product_Provider.Models;

namespace Product_Provider.Function;

public class CreateSubCategory(ILogger<CreateSubCategory> logger, DataContext context)
{
    private readonly ILogger<CreateSubCategory> _logger = logger;
    private readonly DataContext _context = context;

	[Function("CreateSubCategory")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
		try
		{
			var body = await new StreamReader(req.Body).ReadToEndAsync();
			var subCategory = JsonConvert.DeserializeObject<SubCategoryModel>(body);

			var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName == subCategory.CategoryName);
			var categoryId = category!.Id;

			if (!string.IsNullOrEmpty(categoryId))
			{
				var subCategoryEntity = new SubCategory 
				{ 
					SubCategoryName = subCategory.SubCategoryName,
					CategoryId = categoryId,
				};
				if(subCategoryEntity != null) 
				{
					_context.SubCategories.Add(subCategoryEntity);
					await _context.SaveChangesAsync();
					return new OkResult();
				}

			}

			
			return new NotFoundObjectResult("Category not found");
		}
		catch (Exception ex)
		{
			_logger.LogError($"ERROR : CreateSubCategory.Run() :: {ex.Message}");
		}

		return new BadRequestResult();
	}
}
