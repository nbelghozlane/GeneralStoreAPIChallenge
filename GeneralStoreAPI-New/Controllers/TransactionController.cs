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
    public class TransactionController : ApiController
    {
        private readonly GeneralStoreDbContext _context = new GeneralStoreDbContext();

        //Post
        [HttpPost]
        public async Task<IHttpActionResult> PostTransaction([FromBody] Transaction model)
        {
            if (model is null)
                return BadRequest("Your request body cannot be empty.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var findProduct = await _context.Products.FindAsync(model.ProductSKU);
            if (findProduct is null)
                return BadRequest($"The target product with the SKU of {model.ProductSKU} does not exist.");

            //verify that product is in stock and there is enough product for transaction
            if (findProduct.IsInStock == true && findProduct.NumberInInventory >= model.ItemCount)
            {
                //Add transaction
                _context.Transactions.Add(model);
                await _context.SaveChangesAsync();

                //Remove products that were bought from product inventory
                int RemoveBoughtProducts = findProduct.NumberInInventory - model.ItemCount;//
                findProduct.NumberInInventory = RemoveBoughtProducts;
                await _context.SaveChangesAsync();//
                
                return Ok($"You created a transaction successfully!");
            }
            return BadRequest(ModelState);

        }

        //Get all transactions
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Transaction> transactions = await _context.Transactions.ToListAsync(); 
            return Ok(transactions);
        }

        //GetbyCustomerID
        [HttpGet]
        public async Task<IHttpActionResult> GetByCustomerId([FromUri] int CustomerId)
        {
            Transaction transaction = await _context.Transactions.FindAsync(CustomerId);

            if (transaction != null)
            {
                return Ok(transaction);
            }

            return NotFound();
        }

        //GetByTransactionID - doesnt work??
        [HttpGet]
        public async Task<IHttpActionResult> GetByTransactionId([FromUri] int id)
        {
            Transaction transaction = await _context.Transactions.FindAsync(id);

            if (transaction != null)
            {
                return Ok(transaction);
            }

            return NotFound();
        }

        //Put (update) by transaction ID

        [HttpPut]
        public async Task<IHttpActionResult> UpdateTransaction([FromUri] int id, [FromBody] Transaction updatedTransaction)
        {
            //Check to see if IDs match
            if (id != updatedTransaction?.Id)  
            {
                return BadRequest("Ids do not match.");
            }

            //Check the ModelState
            if (!ModelState.IsValid)  
                return BadRequest(ModelState);

            //Find the restaurant in the database
            Transaction transaction = await _context.Transactions.FindAsync(id);

            //If the restaurant doesn't exist then do something
            if (transaction is null)
                return NotFound();

            //Update the properties
            transaction.Id = updatedTransaction.Id;
            transaction.CustomerId = updatedTransaction.CustomerId;
            transaction.ProductSKU = updatedTransaction.ProductSKU;
            transaction.ItemCount = updatedTransaction.ItemCount;
            transaction.DateOfTransaction = updatedTransaction.DateOfTransaction;

            //Save changes
            await _context.SaveChangesAsync();

            return Ok("The restaurant was updated!");

            //verify product changes

            //update product inventory to reflect updated transaction
        }

        //Delete by ID

        //update product inventory to reflect updated transaction
    }

}
