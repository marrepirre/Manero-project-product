using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Product_Provider.Data.Contexts;
using Product_Provider.Data.Entities;

namespace Product_Provider.Function
{
    public class CreateSize(ILogger<CreateSize> logger, DataContext context)
	{
        private readonly ILogger<CreateSize> _logger = logger;
        private readonly DataContext _context = context;

		[Function("CreateSize")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
			try
			{
				var body = await new StreamReader(req.Body).ReadToEndAsync();
				var size = JsonConvert.DeserializeObject<SizeEntity>(body);

				_context.Sizes.Add(size);
				await _context.SaveChangesAsync();

				return new OkResult();
			}
			catch (Exception ex)
			{
				_logger.LogError($"ERROR : CreateSize.Run() :: {ex.Message}");
			}

			return new BadRequestResult();
		}
    }
}
