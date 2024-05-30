using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Product_Provider.Data.Contexts;
using Product_Provider.Data.Entities;
using Product_Provider.Models;

namespace Product_Provider.Function;

public class GetProductById(ILogger<GetProductById> logger, DataContext context)
{
    private readonly ILogger<GetProductById> _logger = logger;
    private readonly DataContext _context = context;

    [Function("GetProductById")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id}")] HttpRequest req, string id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                _logger.LogError($"Product with ID: {id} not found.");
                return new NotFoundResult();
            }

            var productModel = MapToProductModel(product);

            return new OkObjectResult(productModel);
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : GetProductById.Run() :: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    public static ProductModel MapToProductModel(ProductEntity productEntity)
    {
        return new ProductModel
        {
            Id = productEntity.Id,
            BatchNumber = productEntity.BatchNumber,
            ProductName = productEntity.ProductName,
            ProductDescription = productEntity.ProductDescription,
            Color = productEntity.Color,
            Size = productEntity.Size,
            Stock = productEntity.Stock,
            IsBestSeller = productEntity.IsBestSeller,
            IsNew = productEntity.IsNew,
            IsSale = productEntity.IsSale,
            IsTop = productEntity.IsTop,
            OriginalPrice = productEntity.OriginalPrice,
            DiscountPrice = productEntity.DiscountPrice,
            Category = productEntity.Category,
            ThumbnailImage = productEntity.ThumbnailImage,
            Images = productEntity.Images
        };
    }
}



