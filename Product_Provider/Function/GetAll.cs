using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Product_Provider.Data.Contexts;
using Product_Provider.Data.Entities;
using Product_Provider.Models;

namespace Product_Provider.Function
{
    public class GetAll(ILogger<GetAll> logger, DataContext context)
	{
        private readonly ILogger<GetAll> _logger = logger;
        private readonly DataContext _context = context;

		[Function("GetAll")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
			try
			{
				var category = req.Query["category"].ToString();
				var color = req.Query["color"].ToString();
				var size = req.Query["size"].ToString();
				var isBestSeller = req.Query["bestseller"].ToString();
                var isSale = req.Query["sale"].ToString();
                var isNew = req.Query["new"].ToString();
                var isTop = req.Query["top"].ToString();


				var query = _context.Products.AsQueryable();

				if (!string.IsNullOrEmpty(category))
					query.Where(x => x.Category == category);

				if (!string.IsNullOrEmpty(color))
                    query.Where(x => x.Color == color);

				if (!string.IsNullOrEmpty(size))
					query.Where(x => x.Size == size);

                if (!string.IsNullOrEmpty(isBestSeller))
                    query.Where(x => x.IsBestSeller == bool.Parse(isBestSeller));

                if (!string.IsNullOrEmpty(isSale))
                    query.Where(x => x.IsSale == bool.Parse(isSale));

                if (!string.IsNullOrEmpty(isNew))
                    query.Where(x => x.IsNew == bool.Parse(isNew));

                if (!string.IsNullOrEmpty(isTop))
                    query.Where(x => x.IsTop == bool.Parse(isTop));

				var items = await query.ToListAsync();

                var products = ProductMapper.ToProductModelList(items);

                return new OkObjectResult(products);
			}
			catch (Exception ex)
			{
				_logger.LogError($"ERROR : GetAll.Run() :: {ex.Message}");
			}

			return new BadRequestResult();
		}


    }


	public static class ProductMapper
	{
        public static List<ProductModel> ToProductModelList(this List<ProductEntity> productEntities)
        {
            return productEntities.Select(pe => new ProductModel
            {
                Id = pe.Id,
                BatchNumber = pe.BatchNumber,
                ProductName = pe.ProductName,
                ProductDescription = pe.ProductDescription,
                Color = pe.Color, // Om du har flera färger, justera denna logik
                Size = pe.Size,   // Om du har flera storlekar, justera denna logik
                Stock = pe.Stock,
                IsBestSeller = pe.IsBestSeller,
                IsNew = pe.IsNew,
                IsSale = pe.IsSale,
                IsTop = pe.IsTop,
                OriginalPrice = pe.OriginalPrice,
                DiscountPrice = pe.DiscountPrice,
                Category = pe.Category,
                ThumbnailImage = pe.ThumbnailImage,
                Images = pe.Images
            }).ToList();
        }
    }
}
