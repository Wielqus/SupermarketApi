using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supermarket.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Supermarket.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        private readonly SupermarketContext _context;

        /// <summary>
        /// Products controller
        /// </summary>
        public ProductsController(SupermarketContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get list of products.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Products
        ///
        /// </remarks>
        /// <returns>A list of products</returns>
        /// <response code="200">Returns the products list</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Get product
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Products/1
        ///
        /// </remarks>
        /// <returns>Product</returns>
        /// <response code="200">Returns the product</response>
        /// <response code="404">Product not find</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Products/1
        ///     {
        ///        "name": "Banan",
        ///        "price": 2
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly updated product</returns>
        /// <response code="201">Returns the newly updated product</response>
        /// <response code="400">Bad request</response> 
        /// <response code="404">Product not find</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(long id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a Product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Products
        ///     {
        ///        "name": "Banan",
        ///        "price": 2
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created prodict</returns>
        /// <response code="201">Returns the newly created product</response>
        /// <response code="400">Bad request</response>            
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        /// <summary>
        /// Delete product
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/Products/1
        ///
        /// </remarks>
        /// <returns>A newly deleted product</returns>
        /// <response code="201">Returns the newly deleted product</response>
        /// <response code="404">Product not find</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
