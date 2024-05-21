using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Product_Provider.Data.Contexts;

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
				var batch = req.Query["batch"].ToString();
				var isBestSeller = req.Query["bestseller"].ToString();
                var isSale = req.Query["sale"].ToString();
                var isNew = req.Query["new"].ToString();
                var isTop = req.Query["top"].ToString();
                var query = _context.Products
				.Include(i => i.Subcategory).ThenInclude(i => i.Category)
				.Include(i => i.Color)
				.Include(i => i.Size)
				.AsQueryable();

				if (!string.IsNullOrEmpty(batch))
				{
					query = query.Where(x => x.BatchNumber == batch);
				}

				if (!string.IsNullOrEmpty(category))
				{
					query = query.Where(x => x.Subcategory.Category.CategoryName == category);
				}

				if (!string.IsNullOrEmpty(color))
				{
					query = query.Where(x => x.Color == color);
				}

				if (!string.IsNullOrEmpty(size))
				{
					query = query.Where(x => x.Size == size);
				}

				if (isBestSeller == "true")
				{
					query = query.Where(x => x.IsBestSeller == true);
				}

                if (isSale == "true")
                {
                    query = query.Where(x => x.IsSale == true);
                }

                if (isNew == "true")
                {
                    query = query.Where(x => x.IsNew == true);
                }

                if (isTop == "true")
                {
                    query = query.Where(x => x.IsTop == true);
                }

                var products = await query.ToListAsync();

				return new OkObjectResult(products);
			}
			catch (Exception ex)
			{
				_logger.LogError($"ERROR : GetAll.Run() :: {ex.Message}");
			}

			return new BadRequestResult();
		}
    }
}
