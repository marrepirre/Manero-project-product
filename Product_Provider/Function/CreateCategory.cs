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

public class CreateCategory(ILogger<CreateCategory> logger, DataContext context)
{
    private readonly ILogger<CreateCategory> _logger = logger;
    private readonly DataContext _context = context;

	[Function("CreateCategory")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
		try
		{

			var body = await new StreamReader(req.Body).ReadToEndAsync();
			var request = JsonConvert.DeserializeObject<CategoryRequest>(body);

			var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName == request.CategoryName);
			if (category == null)
			{
				category = new CategoryEntity
				{
					CategoryName = request.CategoryName
				};

				_context.Add(category);
				await _context.SaveChangesAsync();
			}

            foreach (var subCategory in request.SubCategories!)
            {
                category.SubCategories.Add(subCategory);
            }

            _context.Entry(category).State = EntityState.Modified;
			await _context.SaveChangesAsync();

            return new OkResult();

		}
		catch (Exception ex)
		{
			_logger.LogError($"ERROR : CreateCategory.Run() :: {ex.Message}");
		}

		return new BadRequestResult();
	}
}
