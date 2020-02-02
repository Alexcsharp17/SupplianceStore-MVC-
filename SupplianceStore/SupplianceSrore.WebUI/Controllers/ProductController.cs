using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.WebUI.Models;
using SupplianceSrore.WebUI.Models;
using SupplianceStore.Domain.Abstract;
using SupplianceStore.Domain.Entities;

namespace SupplianceSrore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        private IProductRepository repository;
        public int pageSize = 12;
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }
        public ViewResult List(string category, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(product => product.ProductId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                repository.Products.Count() :
                repository.Products.Where(product => product.Category == category).Count()
                },
                CurrentCategory = category
            };
            Session["ser-rec"] = "$";
            return View(model);
        }
        [HttpPost]
        public ViewResult List(string category, string ser_rec = null, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                    .Where(p => p.Name.ToLower().Replace(" ","").Contains(ser_rec.ToLower().Replace(" ", "")) )
                    .OrderBy(game => game.ProductId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                repository.Products.Count() :
                repository.Products.Where(product => product.Category == category).Count()
                },
                CurrentCategory = category
            };
            Session["ser-rec"] = ser_rec.Replace(" ","$");
            return View(model);
        }
        public ViewResult Edit(int productId)
        {
            Product product = repository.Products
                .FirstOrDefault(g => g.ProductId == productId);
            return View(product);
        }

        // Перегруженная версия Edit() для сохранения изменений
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["message"] = string.Format("Изменения в товаре \"{0}\" были сохранены", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(product);
            }

        }
        public FileContentResult GetImage(int productId)
        {
            Product product = repository.Products
                .FirstOrDefault(g => g.ProductId == productId);

            if (product != null)
            {
                return File(product.ImageData, product.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public ActionResult ProdDetails(int productId)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductId == productId);
            return View(product);
        }

    }
}
