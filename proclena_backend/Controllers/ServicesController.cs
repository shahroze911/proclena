using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using proclena_backend.Models;

namespace proclena_backend.Controllers
{
    public class ServicesController : Controller
    {
        private proclenaEntities db = new proclenaEntities();

        // GET: Services
        public ActionResult Index()
        {
            return View(db.Services.ToList());
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                {
                    // Convert the image to a byte array
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(model.ImageFile.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(model.ImageFile.ContentLength);
                    }

                    // Create Service object
                    var service = new Service
                    {
                        CategoryName = model.CategoryName,
                        ImageData = imageData // Store the image data
                    };

                    // Save to Database
                    db.Services.Add(service);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Please select an image file.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }

            var model = new ServiceViewModel
            {
                Id = service.Id,
                CategoryName = service.CategoryName,
                ImageData = service.ImageData
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var service = db.Services.Find(model.Id);
                if (service == null)
                {
                    return HttpNotFound();
                }

                // Handle Image Upload (if a new image is uploaded)
                if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        model.ImageFile.InputStream.CopyTo(memoryStream);
                        service.ImageData = memoryStream.ToArray();
                    }
                }

                // Update other fields
                service.CategoryName = model.CategoryName;

                // Mark the entity as modified and save changes
                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
