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
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class CreateColorTests
{
    private readonly Mock<ILogger<CreateColor>> _mockLogger;
    private readonly DataContext _context;
    private readonly CreateColor _function;

    public CreateColorTests()
    {
        _mockLogger = new Mock<ILogger<CreateColor>>();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new DataContext(options);

        _function = new CreateColor(_mockLogger.Object, _context);
    }

    [Fact]
    public async Task Run_Should_Create_Color_And_Return_OkResult()
    {
        // Arrange
        var newColor = new ColorRequest
        {
            Color = "Red"
        };

        var jsonColor = JsonConvert.SerializeObject(newColor);
        var request = new DefaultHttpContext().Request;
        request.Body = new MemoryStream(Encoding.UTF8.GetBytes(jsonColor));

        // Act
        var result = await _function.Run(request);

        // Assert
        Assert.IsType<OkResult>(result);

        // Verify that the color is created in the database
        var color = await _context.Colors.FirstOrDefaultAsync(c => c.Color == "Red");
        Assert.NotNull(color);
    }

    [Fact]
    public async Task Run_Should_Return_BadRequest_When_Color_Already_Exists()
    {
        // Arrange: Add a color to the in-memory database
        var existingColor = new ColorEntity { Color = "Blue" };
        _context.Colors.Add(existingColor);
        await _context.SaveChangesAsync();

        // Create a request with the same color
        var request = new DefaultHttpContext().Request;
        var requestBody = new ColorRequest { Color = "Blue" };
        var jsonRequest = JsonConvert.SerializeObject(requestBody);
        request.Body = new MemoryStream(Encoding.UTF8.GetBytes(jsonRequest));

        // Act
        var result = await _function.Run(request);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}
