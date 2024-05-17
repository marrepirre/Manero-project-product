using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Product_Provider.Data.Contexts;

var host = new HostBuilder()
	.ConfigureFunctionsWebApplication()
	.ConfigureServices(services =>
	{
		services.AddApplicationInsightsTelemetryWorkerService();
		services.ConfigureFunctionsApplicationInsights();
		services.AddDbContext<DataContext>(x => x.UseCosmos(Environment.GetEnvironmentVariable("CosmosDB")!, "Manero"));
	})
	.Build();

host.Run();
