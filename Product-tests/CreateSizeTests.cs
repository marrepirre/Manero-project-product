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

public class CreateSizeTests
{
    private readonly Mock<ILogger<CreateSize>> _mockLogger;
    private readonly DataContext _context;
    private readonly CreateSize _function;

    public CreateSizeTests()
    {
        _mockLogger = new Mock<ILogger<CreateSize>>();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new DataContext(options);

        _function = new CreateSize(_mockLogger.Object, _context);
    }

    [Fact]
    public async Task Run_Should_Create_Size_And_Return_OkResult()
    {
        // Arrange
        var newSize = new SizeRequest
        {
            Size = "Large"
        };

        var jsonSize = JsonConvert.SerializeObject(newSize);
        var request = new DefaultHttpContext().Request;
        request.Body = new MemoryStream(Encoding.UTF8.GetBytes(jsonSize));

        // Act
        var result = await _function.Run(request);

        // Assert
        Assert.IsType<OkResult>(result);

        // Verify that the size is created in the database
        var size = await _context.Sizes.FirstOrDefaultAsync(s => s.Size == "Large");
        Assert.NotNull(size);
    }

    [Fact]
    public async Task Run_Should_Return_BadRequest_When_Size_Already_Exists()
    {
        // Arrange: Add a size to the in-memory database
        var existingSize = new SizeEntity { Size = "Small" };
        _context.Sizes.Add(existingSize);
        await _context.SaveChangesAsync();

        // Create a request with the same size
        var request = new DefaultHttpContext().Request;
        var requestBody = new SizeRequest { Size = "Small" };
        var jsonRequest = JsonConvert.SerializeObject(requestBody);
        request.Body = new MemoryStream(Encoding.UTF8.GetBytes(jsonRequest));

        // Act
        var result = await _function.Run(request);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}
