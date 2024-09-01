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
    public class ProductsController : Controller
    {
        private proclenaEntities db = new proclenaEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products
                .Include(p => p.ProductProperties)
                .ToList(); // Ensure to load all necessary data

            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Include(p => p.ProductProperties).FirstOrDefault(p => p.ProductID == id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }
        public ActionResult GetImage(int id)
        {
            var product = db.Products.SingleOrDefault(p => p.ProductID == id);

            if (product == null || product.ProductImage == null)
            {
                // Handle the case where product or image is not found
                return HttpNotFound();
            }

            // Return the image as a File result
            return File(product.ProductImage, "image/jpeg"); // Adjust content type if needed
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel model, HttpPostedFileBase productImageFile)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    ProductName = model.ProductName,
                    ProductCategory = model.ProductCategory,
                    ProductPrice = model.ProductPrice,
                    ProductDescription = model.ProductDescription,
                    ProductProperties = model.ProductProperties.Select(p => new ProductProperty
                    {
                        Key = p.Key,
                        Value = p.Value
                    }).ToList()
                };

                if (productImageFile != null && productImageFile.ContentLength > 0)
                {
                    // Convert the uploaded image to byte array
                    using (var binaryReader = new System.IO.BinaryReader(productImageFile.InputStream))
                    {
                        product.ProductImage = binaryReader.ReadBytes(productImageFile.ContentLength);
                    }
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }
        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Fetch the product including ProductProperties
            Product product = db.Products.Include(p => p.ProductProperties).SingleOrDefault(p => p.ProductID == id);

            if (product == null)
            {
                return HttpNotFound();
            }

            // Create a ViewModel and populate it
            var model = new ProductViewModel
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                ProductCategory = product.ProductCategory,
                ProductPrice = product.ProductPrice,
                ProductDescription = product.ProductDescription,
                ProductProperties = product.ProductProperties
                    .Select(pp => new PropertyViewModel
                    {
                        Key = pp.Key,
                        Value = pp.Value
                    }).ToList(),
                ExistingProductImage = product.ProductImage // To handle the existing image
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel model, HttpPostedFileBase productImageFile)
        {
            if (ModelState.IsValid)
            {
                var product = db.Products.Include(p => p.ProductProperties)
                                                .SingleOrDefault(p => p.ProductID == model.ProductID);
                if (product != null)
                {
                    product.ProductName = model.ProductName;
                    product.ProductCategory = model.ProductCategory;
                    product.ProductPrice = model.ProductPrice;
                    product.ProductDescription = model.ProductDescription;
                    product.ProductProperties.Clear();

                    foreach (var prop in model.ProductProperties)
                    {
                        product.ProductProperties.Add(new ProductProperty
                        {
                            Key = prop.Key,
                            Value = prop.Value
                        });
                    }

                    if (productImageFile != null)
                    {
                        using (var binaryReader = new BinaryReader(productImageFile.InputStream))
                        {
                            product.ProductImage = binaryReader.ReadBytes(productImageFile.ContentLength);
                        }
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }


        // GET: Products/Delete/5
        // GET: /Product/Delete/5
public ActionResult Delete(int? id)
{
    if (id == null)
    {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    }

    // Fetch the product to delete
    Product product = db.Products.Find(id);
    if (product == null)
    {
        return HttpNotFound();
    }

    return View(product); // Return the delete confirmation view
}

// POST: /Product/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public ActionResult DeleteConfirmed(int id)
{
    var product = db.Products.Find(id);

    if (product == null)
    {
        return HttpNotFound();
    }

    // Update ProductProperties to reflect deletion
    var productProperties = db.ProductProperties.Where(pp => pp.ProductID == id).ToList();
    foreach (var property in productProperties)
    {
        db.ProductProperties.Remove(property); // Remove related properties
    }

    // Remove the product itself
    db.Products.Remove(product);

    db.SaveChanges(); // Save changes to the database

    return RedirectToAction("Index"); // Redirect to the index page after deletion
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
