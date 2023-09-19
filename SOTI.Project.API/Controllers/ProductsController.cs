using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SOTI.Project.DAL;
using System.Runtime.Remoting.Messaging;
using SOTI.Project.API.Custom_Filter;
using System.Web.Http.Cors;

namespace SOTI.Project.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/SOTI/Products")]
    public class ProductsController : ApiController
    {
        //public IHttpActionResult GetData() //we can return in json, string, status code format
        //{
        //    return Json(new { Message = "Konnichiwa" });
        //}

        private readonly IProduct _product = null;
        private readonly IProductAdditional _additional = null;

        public ProductsController(IProduct product, IProductAdditional additional)
        {
            _product = product;
            _additional = additional;
            // _product = new ProductDetailsConnect();
        }
        [HttpGet]
        [Route("all")]
        //[BasicAuthentication]
        [AllowAnonymous]
        public IHttpActionResult GetProducts()
        {
            var ds = _product.GetAllProduct();
            if (ds == null)
            {
                return BadRequest();
            }
            else return Ok(ds);


        }


        [HttpGet]
       // [BasicAuthentication]
        [Route("{productId}/orders")] // https://localhost/port/api/soti/?prodcutId=1/orders
       
        public IHttpActionResult GetProductById([FromUri] int productId)
        {
            var dt = _product.GetProductById(productId);
            if (dt == null)
            {
                return BadRequest();
            }
            return Ok(dt);
        }

        [HttpGet]
        [Route("ByPrice/{price?}")]
        public IHttpActionResult ProductByPrice(decimal? price)
        {
            if (price.HasValue)
            {
                var products = _additional.GetProductByPrice(price.Value);

                if (products == null)
                {
                    return NotFound();
                }

                return Ok(products);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("addproduct")]
        [Authorize (Roles = "Admin, Employee")]
        public IHttpActionResult AddProduct(Product product)
        {
            var result = _product.AddProduct(product.ProductName, product.UnitPrice.Value, product.UnitsInStock.Value);
            if (result)
            {
                //return StatusCode(HttpStatusCode.NoContent);
                return Created("api/SOTI/Products/" + product.ProductId, product); //we want to return the id to the user so that he knows what id is created when product got added
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateProduct([FromUri]int id, [FromBody]Product product)
        {
            if(id != product.ProductId)
            {
                return BadRequest();
            }
            var result = _product.UpdateProduct(id, product);
            if (result)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return BadRequest();
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteProduct([FromUri]int id)
        {
            var res = _product.DeleteProduct(id);
            if (res)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
