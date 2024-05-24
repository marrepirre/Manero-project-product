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
				var request = JsonConvert.DeserializeObject<ProductRequest>(body);

				if (request != null)
				{
					var productEntity = new ProductEntity
					{
						BatchNumber = request.BatchNumber,
						ProductName = request.ProductName,
						ProductDescription = request.ProductDescription,
						Color = request.Color,
						Size = request.Size,
						ThumbnailImage = request.ThumbnailImage,
						Stock = request.Stock,
						IsNew = request.IsNew,
						IsSale = request.IsSale,
						IsTop = request.IsTop,
						IsBestSeller = request.IsBestSeller,
						OriginalPrice = request.OriginalPrice,
						DiscountPrice = request.DiscountPrice,
						SubCategory = request.SubCategory,
						Images = request.Images,
					};
  
					_context.Add(productEntity);
					await _context.SaveChangesAsync();
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
