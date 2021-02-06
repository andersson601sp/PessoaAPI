using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapicore.Models;
using webapicore.Data;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Cors;

namespace webapicore.Controllers
{
   [ApiController]
   [Route("v1/products")]

   [EnableCors("AllowOrigin")]
    public class Productontroller : ControllerBase
   {
      [HttpGet]
       [Route("")]
       public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
       {
           var products = await context.Products.Include(x => x.Category).ToListAsync();
           return products;
       }

       [HttpGet]
       [Route("{id:int}")]
       public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
       {
           var product   = await context.Products.Include(x => x.Category)
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.Id == id);
           return product;
       }

       [HttpGet]
       [Route("categories/{id:int}")]
       public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
       {
           var products = await context.Products
           .Include(x => x.Category)
           .AsNoTracking()
           //.where(x => x.CategoryId == id)
           .ToListAsync();
           return products;
       }


       [HttpPost]
       [Route("")]
       public async Task<ActionResult<Product>> Post(
           [FromServices] DataContext context,
           [FromBody]Product model)
           {
               if(ModelState.IsValid){
                   context.Products.Add(model);
                   await context.SaveChangesAsync();
                   return model;
               }
               else
               {
                  return BadRequest(ModelState);
               }
           }
       
   }
}