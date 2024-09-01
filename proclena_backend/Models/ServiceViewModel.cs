using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proclena_backend.Models
{
    public class ServiceViewModel
    {
        public int Id { get; set; }               
        public string CategoryName { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public byte[] ImageData { get; set; } // Add this line
    }
}