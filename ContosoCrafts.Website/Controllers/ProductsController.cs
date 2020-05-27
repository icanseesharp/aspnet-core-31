using System.Collections.Generic;
using ContosoCrafts.Website.Models;
using ContosoCrafts.Website.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoCrafts.Website.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        JsonFileProductService ProductService {get;}
        public ProductsController(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return ProductService.GetProducts();    
        }

        [HttpGet]
        [Route("Rate")]
        public ActionResult Get(
            [FromQuery] string productId, 
            [FromQuery] int rating)
        {
            ProductService.AddRating(productId, rating);
            return Ok();
        }
    }
}
