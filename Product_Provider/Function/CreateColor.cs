using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Product_Provider.Data.Contexts;
using Product_Provider.Data.Entities;

namespace Product_Provider.Function
{
    public class CreateColor(ILogger<CreateColor> logger, DataContext context)
	{
        private readonly ILogger<CreateColor> _logger = logger;
        private readonly DataContext _context = context;

		[Function("CreateColor")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
			try
			{
				var body = await new StreamReader(req.Body).ReadToEndAsync();
				var color = JsonConvert.DeserializeObject<ColorEntity>(body);

				_context.Colors.Add(color);
				await _context.SaveChangesAsync();

				return new OkResult();
			}
			catch (Exception ex)
			{
				_logger.LogError($"ERROR : CreateColor.Run() :: {ex.Message}");
			}

			return new BadRequestResult();
		}
    }
}
