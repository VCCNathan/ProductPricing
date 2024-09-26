using Microsoft.AspNetCore.Mvc;
using ProductPricing.Shared.Models;
using ProductPricing.Shared.Requests;

namespace ProductPricing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Product>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Product>> AllProducts()
    {
        var products = DataStore.Products.Select(p => p).ToList();

        if (products.Count == 0)
        {
            return NotFound();
        }

        return Ok(products);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetProductById(int id)
    {
        var product = DataStore.Products.Find(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost("{id}/apply-discount")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ApplyDiscount(int id, [FromBody] DiscountRequest discountRequest)
    {
        var product = DataStore.Products.Find(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        var discountedPrice = Math.Round(product.CurrentPrice * (1 - discountRequest.DiscountPercentage / 100.0m), 2);

        product.PriceHistory!.Add(new PriceHistory { Price = product.CurrentPrice, Date = DateTime.Now });
        product.CurrentPrice = discountedPrice;
        product.LastUpdated = DateTime.Now;

        return Ok(new
        {
            product.Id,
            product.Name,
            OriginalPrice = product.PriceHistory[^1].Price,
            DiscountedPrice = discountedPrice
        });
    }

    [HttpPut("{id}/update-price")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdatePrice(int id, [FromBody] UpdatePriceRequest updatePriceRequest)
    {
        var product = DataStore.Products.Find(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        product.PriceHistory!.Add(new PriceHistory { Price = product.CurrentPrice, Date = DateTime.Now });

        product.CurrentPrice = Math.Round(updatePriceRequest.NewPrice, 2);
        product.LastUpdated = DateTime.Now;

        return Ok(new
        {
            product.Id,
            product.Name,
            OriginalPrice = product.PriceHistory[^1].Price,
            NewPrice = product.CurrentPrice,
            product.LastUpdated
        });
    }
}