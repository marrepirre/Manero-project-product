using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Product_Provider.Data.Contexts;
using Product_Provider.Function;
using Product_Provider.Models;
using System.Text;

public class CreateProductTests
{
    private readonly Mock<ILogger<CreateProduct>> _mockLogger;
    private readonly DataContext _context;
    private readonly CreateProduct _function;

    public CreateProductTests()
    {
        _mockLogger = new Mock<ILogger<CreateProduct>>();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new DataContext(options);

        _function = new CreateProduct(_mockLogger.Object, _context);
    }

    [Fact]
    public async Task Run_Should_Create_Product_And_Return_OkResult()
    {
        // Arrange
        var newProduct = new ProductRequest
        {
            BatchNumber = "Batch001",
            ProductName = "Test Product",
            ProductDescription = "Description",
            Color = "Red",
            Size = "L",
            ThumbnailImage = "image.jpg",
            Stock = 10,
            IsNew = true,
            IsSale = false,
            IsTop = false,
            IsBestSeller = false,
            OriginalPrice = 100,
            DiscountPrice = 80,
            Category = "Category1",
            Images = new List<string> { "image1.jpg", "image2.jpg" }
        };

        var jsonProduct = JsonConvert.SerializeObject(newProduct);
        var request = new DefaultHttpContext().Request;
        request.Body = new MemoryStream(Encoding.UTF8.GetBytes(jsonProduct));

        // Act
        var result = await _function.Run(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductName == "Test Product");
        Assert.NotNull(product);
        Assert.Equal(newProduct.ProductName, product.ProductName);
    }

    [Fact]
    public async Task Run_Should_Return_BadRequest_When_Request_Is_Invalid()
    {
        // Arrange
        var request = new DefaultHttpContext().Request;
        request.Body = new MemoryStream(Encoding.UTF8.GetBytes("invalid json"));

        // Act
        var result = await _function.Run(request);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}
