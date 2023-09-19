using SOTI.Project.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SOTI.Project.API.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly ICustomer _customer = null;

        public CustomerController(ICustomer customer)
        {
            _customer = customer;
            // _product = new ProductDetailsConnect();
        }

        [HttpGet]
        public IHttpActionResult DifferentCountry()
        {
            var dt = _customer.DifferentCountry();
            if (dt == null)
            {
                return BadRequest();
            }
            return Ok(dt);
        }

        //[HttpPost]
        //public IHttpActionResult AddProduct(Product product)
        //{
        //    var result = _product.AddProduct(product.ProductName, product.UnitPrice.Value, product.UnitsInStock.Value);
        //    if (result)
        //    {
        //        return StatusCode(HttpStatusCode.NoContent);
        //    }
        //    return BadRequest();
        //}

        //[HttpPut]
        //public IHttpActionResult UpdateProduct([FromUri] int id, [FromBody] Product product)
        //{
        //    var result = _product.UpdateProduct(id, product);
        //    if (result)
        //    {
        //        return Ok();
        //    }
        //    return BadRequest();
        //}
        //[HttpDelete]
        //public IHttpActionResult DeleteProduct([FromUri] int id)
        //{
        //    var res = _product.DeleteProduct(id);
        //    if (res)
        //    {
        //        return Ok();
        //    }
        //    return BadRequest();
        //}
    }
}
