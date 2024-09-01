using proclena_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace proclena_backend.Controllers
{
    [RoutePrefix("api/services")]
    [EnableCors(origins: "http://127.0.0.1:5500", headers: "*", methods: "*")]
    public class ServicesAPIController : ApiController
    {
        private proclenaEntities db = new proclenaEntities();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetServices()
        {
            var services = db.Services.ToList();

            var result = services.Select(service => new
            {
                service.Id,
                service.CategoryName,
                ImageData = Convert.ToBase64String(service.ImageData) // Convert image data to Base64 string
            });

            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetService(int id)
        {
            var service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            var result = new
            {
                service.Id,
                service.CategoryName,
                ImageData = Convert.ToBase64String(service.ImageData) // Convert image data to Base64 string
            };

            return Ok(result);
        }
    }
}
