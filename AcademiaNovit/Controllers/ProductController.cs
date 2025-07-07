using AcademiaNovit.Models;
using AcademiaNovit.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AcademiaNovit.Controllers;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductController(AppDbContext db)
    {
        _db = db;       
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        Log.Information("Fetching all products");
        var products = await _db.Products.ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        Log.Information("Fetching product with ID {ProductId}", id);
        var product = await _db.Products.FindAsync(id);
        if (product == null)
        {
            Log.Information("Product with ID {ProductId} not found", id);
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        var validation = ProductValidator.ValidateProduct(product.Name, product.Price);
        if (!validation.IsValid)
        {
            Log.Information("Invalid product creation attempt: {ErrorMessage}", validation.ErrorMessage);
            return BadRequest(validation.ErrorMessage);
        }
        Log.Information("Creating new product: {ProductName}", product.Name);
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
    {
        var validation = ProductValidator.ValidateProduct(updatedProduct.Name, updatedProduct.Price);
        if (!validation.IsValid)
        {
            Log.Information("Invalid product update attempt for ID {ProductId}: {ErrorMessage}", id, validation.ErrorMessage);
            return BadRequest(validation.ErrorMessage);
        }

        var product = await _db.Products.FindAsync(id);
        if (product == null)
        {
            Log.Information("Product with ID {ProductId} not found for update", id);
            return NotFound();
        }

        Log.Information("Updating product with ID {ProductId}", id);
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
        {
            Log.Information("Product with ID {ProductId} not found for deletion", id);
            return NotFound();
        }

        Log.Information("Deleting product with ID {ProductId}", id);
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}