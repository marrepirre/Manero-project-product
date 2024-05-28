using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Product_Provider.Data.Contexts;

namespace Product_Provider.Function;

public class GetProductsByCategory(ILogger<GetProductsByCategory> logger, DataContext context)
{
    private readonly ILogger<GetProductsByCategory> _logger = logger;
    private readonly DataContext _context = context;

    [Function("GetProductsByCategory")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {

        try
        {
            string categoryName = req.Query["category"]!;
            var products = await _context.Products.Where(p => p.Category == categoryName).ToListAsync();

            return new OkObjectResult(products);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving products by category: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
  
    }
}
