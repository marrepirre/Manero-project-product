using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Product_Provider.Data.Contexts;
using Product_Provider.Data.Entities;
using Product_Provider.Function;
using Product_Provider.Models;
using System.Text;


public class CreateCategoryTests
{
    private readonly Mock<ILogger<CreateCategory>> _mockLogger;
    private readonly DataContext _context;
    private readonly CreateCategory _function;

    public CreateCategoryTests()
    {
        _mockLogger = new Mock<ILogger<CreateCategory>>();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new DataContext(options);

        _function = new CreateCategory(_mockLogger.Object, _context);
    }

    [Fact]
    public async Task Run_Should_Create_Category_And_Return_OkResult()
    {
        // Arrange
        var newCategory = new CategoryRequest
        {
            CategoryName = "TestCategory"
        };

        var jsonCategory = JsonConvert.SerializeObject(newCategory);
        var request = new DefaultHttpContext().Request;
        request.Body = new MemoryStream(Encoding.UTF8.GetBytes(jsonCategory));

        // Act
        var result = await _function.Run(request);

        // Assert
        Assert.IsType<OkResult>(result);

        // Verify that the category is created in the database
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == "TestCategory");
        Assert.NotNull(category);
    }

    [Fact]
    public async Task Run_Should_Return_BadRequest_When_Category_Already_Exists()
    {
        // Arrange: Add a category to the in-memory database
        var existingCategory = new CategoryEntity { CategoryName = "ExistingCategory" };
        _context.Categories.Add(existingCategory);
        await _context.SaveChangesAsync();

        // Create a request with the same category name
        var request = new DefaultHttpContext().Request;
        var requestBody = new CategoryRequest { CategoryName = "ExistingCategory" };
        var jsonRequest = JsonConvert.SerializeObject(requestBody);
        request.Body = new MemoryStream(Encoding.UTF8.GetBytes(jsonRequest));

        // Act
        var result = await _function.Run(request);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}
