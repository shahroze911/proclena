using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using proclena_backend.Models;

namespace proclena_backend.Controllers
{
    [RoutePrefix("api/products")]
    [EnableCors(origins: "http://127.0.0.1:5500", headers: "*", methods: "*")]
    public class ProductsApiController : ApiController
    {
        private readonly proclenaEntities db = new proclenaEntities();

        // GET: api/products
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetProducts()
        {
            var products = db.Products
                             .Include(p => p.ProductProperties)
                             .Select(p => new ProductViewModel
                             {
                                 ProductID = p.ProductID,
                                 ProductName = p.ProductName,
                                 ProductCategory = p.ProductCategory,
                                 ProductPrice = p.ProductPrice,
                                 ProductDescription = p.ProductDescription,
                                 ExistingProductImage = p.ProductImage,
                                 ProductProperties = p.ProductProperties
                                     .Select(pp => new PropertyViewModel
                                     {
                                         Key = pp.Key,
                                         Value = pp.Value
                                     }).ToList()
                             })
                             .ToList();

            if (products == null || !products.Any())
            {
                return NotFound(); // Returns 404 if no products are found
            }

            return Ok(products); // Returns 200 with the list of products
        }

        // GET: api/products/{id}
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetProduct(int id)
        {
            var product = db.Products
                            .Include(p => p.ProductProperties)
                            .Where(p => p.ProductID == id)
                            .Select(p => new ProductViewModel
                            {
                                ProductID = p.ProductID,
                                ProductName = p.ProductName,
                                ProductCategory = p.ProductCategory,
                                ProductPrice = p.ProductPrice,
                                ProductDescription = p.ProductDescription,
                                ExistingProductImage = p.ProductImage,
                                ProductProperties = p.ProductProperties
                                    .Select(pp => new PropertyViewModel
                                    {
                                        Key = pp.Key,
                                        Value = pp.Value
                                    }).ToList()
                            })
                            .FirstOrDefault();

            if (product == null)
            {
                return NotFound(); // Returns 404 if the product is not found
            }

            return Ok(product); // Returns 200 with the product data
        }
    }
}
