using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Product_Provider.Data.Contexts;
using Product_Provider.Data.Entities;
using Product_Provider.Function;
using Product_Provider.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class GetAllTests : IDisposable
{
    private readonly Mock<ILogger<GetAll>> _mockLogger;
    private readonly DataContext _context;
    private readonly GetAll _function;

    public GetAllTests()
    {
        _mockLogger = new Mock<ILogger<GetAll>>();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unik databasnamn för varje testklass
            .Options;
        _context = new DataContext(options);

        _context.Products.AddRange(new List<ProductEntity>
        {
            new ProductEntity
            {
                BatchNumber = "Batch001",
                ProductName = "Product 1",
                ProductDescription = "Description 1",
                Color = "Red",
                Size = "Large",
                Stock = 10,
                IsBestSeller = false,
                IsNew = true,
                IsSale = false,
                IsTop = false,
                OriginalPrice = 100.0m,
                DiscountPrice = 80.0m,
                Category = "Category1",
                Images = new List<string> { "image1.jpg", "image2.jpg" }
            },
            new ProductEntity
            {
                BatchNumber = "B001",
                ProductName = "Product 2",
                ProductDescription = "Description 2",
                Color = "Red",
                Size = "Medium",
                Stock = 15,
                IsBestSeller = false,
                IsNew = true,
                IsSale = false,
                IsTop = false,
                OriginalPrice = 120.0m,
                DiscountPrice = 90.0m,
                Category = "Category 1",
                Images = new List<string> { "image3.jpg", "image4.jpg" }
            },
            new ProductEntity
            {
                BatchNumber = "B002",
                ProductName = "Product 3",
                ProductDescription = "Description 3",
                Color = "Blue",
                Size = "Small",
                Stock = 20,
                IsBestSeller = true,
                IsNew = false,
                IsSale = true,
                IsTop = false,
                OriginalPrice = 80.0m,
                DiscountPrice = 70.0m,
                Category = "Category 2",
                Images = new List<string> { "image5.jpg", "image6.jpg" }
            }
        });

        _context.SaveChanges();

        _function = new GetAll(_mockLogger.Object, _context);
    }

    public void Dispose()
    {
        // Återställ databasen efter varje test genom att ta bort alla poster
        _context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Run_Should_Return_All_Products()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;

        // Act
        var result = await _function.Run(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var products = Assert.IsType<List<ProductModel>>(okResult.Value);
        Assert.Equal(3, products.Count);
    }
}