using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProductPricing.Api;
using ProductPricing.Api.Controllers;
using ProductPricing.Shared.Models;
using ProductPricing.Shared.Requests;

namespace ProductPricing.Tests;

[TestFixture]
public class ProductControllerTests
{
    [SetUp]
    public void SetUp()
    {
        DataStore.Products =
        [
            new Product
            {
                Id = 1, Name = "Product A", CurrentPrice = 100m, LastUpdated = DateTime.Now, PriceHistory = []
            },
            new Product
            {
                Id = 2, Name = "Product B", CurrentPrice = 200m, LastUpdated = DateTime.Now.AddDays(-1),
                PriceHistory = []
            }
        ];

        _controller = new ProductController();
    }

    private ProductController _controller;

    [Test]
    public void AllProducts_ReturnsOkWithProducts_WhenProductsExist()
    {
        // Act
        var result = _controller.AllProducts();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;

        var products = okResult!.Value;
        products.Should().NotBeNull();
        products.Should().BeEquivalentTo(DataStore.Products);
    }

    [Test]
    public void AllProducts_ReturnsNotFound_WhenNoProductsExist()
    {
        // Arrange
        DataStore.Products.Clear();

        // Act
        var result = _controller.AllProducts();

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public void GetProductById_ReturnsOkWithProduct_WhenProductExists()
    {
        // Act
        var result = _controller.GetProductById(1);

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult!.Value.Should().NotBeNull();

        var product = okResult.Value as Product;
        product.Should().NotBeNull();
        product.Should()
            .BeEquivalentTo(DataStore.Products.Find(p => p.Id == 1));
    }

    [Test]
    public void GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Act
        var result = _controller.GetProductById(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public void ApplyDiscount_ReturnsOkWithDiscountedPrice_WhenProductExists()
    {
        // Act
        var result = _controller.ApplyDiscount(1, new DiscountRequest { DiscountPercentage = 25 });

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult!.Value.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(new
        {
            Id = 1,
            Name = "Product A",
            OriginalPrice = 100m,
            DiscountedPrice = 75m
        });

        var product = DataStore.Products.Find(p => p.Id == 1);
        product!.CurrentPrice.Should().Be(75m);
        product.PriceHistory.Should().HaveCount(1);
    }

    [Test]
    public void ApplyDiscount_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Act
        var result = _controller.ApplyDiscount(999, new DiscountRequest { DiscountPercentage = 10 });

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public void UpdatePrice_ProductExists_PriceUpdated()
    {
        // Act
        var result = _controller.UpdatePrice(1, new UpdatePriceRequest { NewPrice = 150m });

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();

        okResult!.Value.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(new
        {
            Id = 1,
            Name = "Product A",
            OriginalPrice = 100m,
            NewPrice = 150m
        });
    }

    [Test]
    public void UpdatePrice_ProductNotFound_ReturnsNotFound()
    {
        // Arrange
        var updatePriceRequest = new UpdatePriceRequest { NewPrice = 150m };

        // Act
        var result = _controller.UpdatePrice(999, updatePriceRequest);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}