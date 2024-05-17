using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Product_Provider.Data.Contexts;
using Product_Provider.Data.Entities;

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
				var category = JsonConvert.DeserializeObject<CategoryEntity>(body);

				_context.Categories.Add(category);
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
