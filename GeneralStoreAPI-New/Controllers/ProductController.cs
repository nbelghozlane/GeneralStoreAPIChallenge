using GeneralStoreAPI_New.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStoreAPI_New.Controllers
{
    public class ProductController : ApiController
    {
        private readonly GeneralStoreDbContext _context = new GeneralStoreDbContext();

        [HttpPost]
        public async Task<IHttpActionResult> PostProduct([FromBody] Product model)
        {
            if (model is null)
            {
                return BadRequest("Your request body cannot be empty.");
            }

            //If the model is valid
            if (ModelState.IsValid)
            {
                //Store the model in the database
                _context.Products.Add(model);
                int changeCount = await _context.SaveChangesAsync(); 

                return Ok("Your product has been created!");
            }

            //The model is not valid, reject it
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Product> products = await _context.Products.ToListAsync(); 
            return Ok(products);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] string SKU)
        {
            Product product = await _context.Products.FindAsync(SKU);

            if (product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }

        [HttpPut]
        public async Task<IHttpActionResult> UpdateProduct([FromUri] string SKU, [FromBody] Product updatedProduct)
        {
            //Check to see if IDs match
            if (SKU != updatedProduct?.SKU)  // ? means if the updated restaurant is null, don't access the id & evaluate as false
            {
                return BadRequest("SKUs do not match.");
            }

            //Check the ModelState
            if (!ModelState.IsValid)  // ! if model state is invalid /  **Don't have to include curly braces if statement is a single line**
                return BadRequest(ModelState);

            //Find the product in the database
            Product product = await _context.Products.FindAsync(SKU);

            if (product is null)
                return NotFound();

            //Update the properties
            product.Name = updatedProduct.Name;
            product.Cost = updatedProduct.Cost;
            product.NumberInInventory = updatedProduct.NumberInInventory;

            //Save changes
            await _context.SaveChangesAsync();

            return Ok("The product was updated!");
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProduct([FromUri] string SKU)
        {
            Product product = await _context.Products.FindAsync(SKU);

            if (product is null)
                return NotFound();

            _context.Products.Remove(product);

            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok("The product was deleted.");
            }

            return InternalServerError();
        }
    }
}
