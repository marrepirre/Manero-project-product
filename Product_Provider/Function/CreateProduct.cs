using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Product_Provider.Data.Contexts;
using Product_Provider.Data.Entities;
using Product_Provider.Models;

namespace Product_Provider.Function
{
    public class CreateProduct(ILogger<CreateProduct> logger, DataContext context)
	{
        private readonly ILogger<CreateProduct> _logger = logger;
        private readonly DataContext _context = context;

		[Function("CreateProduct")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
			try
			{
				var body = await new StreamReader(req.Body).ReadToEndAsync();
				var product = JsonConvert.DeserializeObject<ProductModel>(body);

				if (product != null)
				{
					var subCategory = await _context.SubCategories.FirstOrDefaultAsync(x => x.SubCategoryName == product.SubCategoryName);
					var subCategoryId = subCategory!.Id;
					var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == subCategory.CategoryId);
					var categoryId = category!.Id;
					if (!string.IsNullOrEmpty(subCategoryId) && !string.IsNullOrEmpty(categoryId))
					{

						var productEntity = new ProductEntity
						{
							BatchNumber = product.BatchNumber,
							ProductName = product.ProductName,
							ProductDescription = product.ProductDescription,
							Color = product.Color,
							Size = product.Size,
							SmallImage = product.SmallImage,
							Stock = product.Stock,
							IsBestSeller = product.IsBestSeller,
							SubCategoryId = subCategoryId,
							CategoryId = categoryId,
							OriginalPrice = product.OriginalPrice,
							DiscountPrice = product.DiscountPrice,
							BigImage = product.BigImage,
						};

						if(productEntity !=  null)
						{
							_context.Products.Add(productEntity);
							await _context.SaveChangesAsync();

							return new OkResult();
						}
					}
				}


			}
			catch (Exception ex)
			{
				_logger.LogError($"ERROR : CreateColor.Run() :: {ex.Message}");
			}

			return new BadRequestResult();
		}
    }
}
