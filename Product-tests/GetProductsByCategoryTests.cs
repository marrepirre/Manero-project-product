using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Product_Provider.Data.Contexts;
using Product_Provider.Function;
using Product_Provider.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;

public class GetProductsByCategoryTests
{
    private readonly ILogger<GetProductsByCategory> _mockLogger;
    private readonly DataContext _context;
    private readonly GetProductsByCategory _function;

    public GetProductsByCategoryTests()
    {
        // Konfigurera minnesdatabasen
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new DataContext(options);
        _mockLogger = new LoggerFactory().CreateLogger<GetProductsByCategory>();

        // Lägg till testdata i minnesdatabasen
        _context.Products.AddRange(new List<ProductEntity>
        {
            new ProductEntity { Id = "1", BatchNumber = "B001", ProductName = "Product 1", ProductDescription = "Description 1", Color = "Red", Size = "Small" },
            new ProductEntity { Id = "2", BatchNumber = "B002", ProductName = "Product 2", ProductDescription = "Description 2", Color = "Blue", Size = "Medium" },
            new ProductEntity { Id = "3", BatchNumber = "B003", ProductName = "Product 3", ProductDescription = "Description 3", Color = "Green", Size = "Large" }
        });
        _context.SaveChanges();

        _function = new GetProductsByCategory(_mockLogger, _context);
    }

    [Fact]
    public async Task Run_Should_Return_Products_By_Category()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var existingContexts = new DataContext(options))
        {
            // Lägg till testdata i minnesdatabasen
            existingContexts.Products.Add(new ProductEntity { BatchNumber = "Batch1", ProductName = "Product1", ProductDescription = "Description1", Color = "Red", Size = "Small", Category = "TestCategory", /* andra egenskaper */ });
            existingContexts.Products.Add(new ProductEntity { BatchNumber = "Batch2", ProductName = "Product2", ProductDescription = "Description2", Color = "Blue", Size = "Large", Category = "TestCategory", /* andra egenskaper */ });
            existingContexts.SaveChanges();
        }

        var mockLogger = new Mock<ILogger<GetProductsByCategory>>();
        var contextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var contexts = new DataContext(contextOptions);
        var request = new DefaultHttpContext().Request;
        request.Query = new QueryCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
    {
        { "category", "TestCategory" }
    });

        var function = new GetProductsByCategory(mockLogger.Object, contexts);

        // Act
        var response = await function.Run(request);

        // Assert
        Assert.IsType<OkObjectResult>(response);
        var result = response as OkObjectResult;
        Assert.NotNull(result);
        var productsReturned = result.Value as List<ProductEntity>;
        Assert.NotNull(productsReturned);
        Assert.Equal(2, productsReturned.Count);
    }
}
