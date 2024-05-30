using Microsoft.Extensions.Logging;
using Moq;
using Product_Provider.Data.Contexts;
using Product_Provider.Data.Entities;
using Product_Provider.Function;

public class GetProductByIdTests
{
    private readonly Mock<ILogger<GetProductById>> _mockLogger;
    private readonly GetProductById _function;

    public GetProductByIdTests()
    {
        _mockLogger = new Mock<ILogger<GetProductById>>();
        _function = new GetProductById(_mockLogger.Object, context: null);
    }

    [Fact]
    public void MapToProductModel_Should_Map_ProductEntity_To_ProductModel_Correctly()
    {
        // Arrange
        var productEntity = new ProductEntity
        {
            Id = "1",
            BatchNumber = "B001",
            ProductName = "Test Product",
            ProductDescription = "Description",
            Color = "Red",
            Size = "L",
            Stock = 10,
            IsBestSeller = true,
            IsNew = false,
            IsSale = true,
            IsTop = false,
            OriginalPrice = 100,
            DiscountPrice = 80,
            Category = "Category1",
            ThumbnailImage = "image.jpg",
            Images = new List<string> { "image1.jpg", "image2.jpg" }
        };

        // Act
        var productModel = GetProductById.MapToProductModel(productEntity);

        // Assert
        Assert.NotNull(productModel);
        Assert.Equal(productEntity.Id, productModel.Id);
        Assert.Equal(productEntity.BatchNumber, productModel.BatchNumber);
        Assert.Equal(productEntity.ProductName, productModel.ProductName);
        Assert.Equal(productEntity.ProductDescription, productModel.ProductDescription);
        Assert.Equal(productEntity.Color, productModel.Color);
        Assert.Equal(productEntity.Size, productModel.Size);
        Assert.Equal(productEntity.Stock, productModel.Stock);
        Assert.Equal(productEntity.IsBestSeller, productModel.IsBestSeller);
        Assert.Equal(productEntity.IsNew, productModel.IsNew);
        Assert.Equal(productEntity.IsSale, productModel.IsSale);
        Assert.Equal(productEntity.IsTop, productModel.IsTop);
        Assert.Equal(productEntity.OriginalPrice, productModel.OriginalPrice);
        Assert.Equal(productEntity.DiscountPrice, productModel.DiscountPrice);
        Assert.Equal(productEntity.Category, productModel.Category);
        Assert.Equal(productEntity.ThumbnailImage, productModel.ThumbnailImage);
        Assert.Equal(productEntity.Images, productModel.Images);
    }
}



