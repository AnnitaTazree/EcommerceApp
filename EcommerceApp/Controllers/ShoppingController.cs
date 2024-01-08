using EcommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Security.Cryptography;

namespace EcommerceApp.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly EcommerceDBContext _dbContext;
        private readonly IWebHostEnvironment _hostEnv;
        public ShoppingController(EcommerceDBContext dbContext, IWebHostEnvironment hostEnv)
        {
            _dbContext = dbContext;
            _hostEnv = hostEnv;

        }
        public IActionResult Index()
        {
            return View(_dbContext.Products.ToList());
        }
        public IActionResult Details(int id)
        {
            return View(_dbContext.Products.FirstOrDefault(p => p.Id == id));
        }
        public IActionResult AddToCart(int productId, double productQuantity)
        {
            bool productFound = false;
            string msg = "";
            if (productId > 0 && productQuantity > 0)
            {
                var product = _dbContext.Products.FirstOrDefault(p=>p.Id == productId);

                if (product != null)
                {
                    List<Product> products = new List<Product>();
                    var previousProductQuantity = HttpContext.Session.GetObject<List<Product>>("cartQuantity");
                    if (previousProductQuantity != null)
                    {
                        foreach (var item in previousProductQuantity)
                        {
                            if (productId == item.Id)
                            {
                                item.Quantity += productQuantity;
                                productFound = true;
                                msg = "Product alredy added, new quantity is added!!!";
                            }
                            products.Add(item);
                        }
                        if (!productFound)
                        {
                            product.Quantity = productQuantity;
                            products.Add(product);
                            msg = "New item is added with existing items!!";
                        }
                        HttpContext.Session.SetObject<List<Product>>("cartQuantity", products);
                    }
                    else
                    {
                        product.Quantity = productQuantity;
                        products.Add(product);
                        HttpContext.Session.SetObject<List<Product>>("cartQuantity", products);
                        msg = "New Item is added to empty cart!!!";
                    }
                }
                else
                {
                    msg = "Item is not found!!!";
                }
            }
            else
            {
                msg = "Item id or qty could not be zero!!!";
            }
            TempData["msg"] = msg;
            return RedirectToAction("Index");
        }
        public IActionResult CartDetails()
        {
            List<Product> products = HttpContext.Session.GetObject<List<Product>>("cartQuantity");
            if (products != null && products.Count != 0)
            {
                return View(products.ToList());
            }
            else
            {
                products = new List<Product>();
                return View();
            }
        }
        public IActionResult UpdateCart(int productId, double productQuantity)
        {
            if (productId > 0 && productQuantity > 0)
            {
                var product = _dbContext.Products.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    List<Product> products = HttpContext.Session.GetObject<List<Product>>("cartQuantity");
                    if (products != null)
                    {
                        var existingProduct = products.FirstOrDefault(p => p.Id == productId);
                        if (existingProduct != null)
                        {
                            existingProduct.Quantity = productQuantity;
                        }
                        HttpContext.Session.SetObject<List<Product>>("cartQuantity", products);
                    }
                }
            }
            return RedirectToAction(nameof(CartDetails));
        }
        public IActionResult RemoveFromCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<Product> productList = HttpContext.Session.GetObject<List<Product>>("cartQuantity");
            var removeItem = productList.FirstOrDefault(x => x.Id == id);
            if(removeItem != null)
            {
                productList.Remove(removeItem);
            }
            HttpContext.Session.SetObject("cartQuantity", productList);
            return RedirectToAction(nameof(CartDetails));
        }
        public IActionResult CheckOut()
        {
            var us = HttpContext.Session.GetString("un");
            var id = HttpContext.Session.GetString("id");
            if (us != null)
            {
                return RedirectToAction(nameof(ConfirmOrder));
            }
            else
            {
                //return View(nameof(Login));
                return RedirectToAction("Login", "Login");
            }
            //return RedirectToAction(nameof(ConfirmOrder));
        }
        public IActionResult ConfirmOrder()
        {
            List<Product> products = HttpContext.Session.GetObject<List<Product>>("cartQuantity");
            if (products != null && products.Count != 0)
            {
                return View(products.ToList());
            }
            else
            {
                products = new List<Product>();
                return View();
            }
        }
        [HttpPost]
        public IActionResult UpdateQuantities(int[] productIds, int[] qtys)
        {
            List<Product> updatedProducts = new List<Product>();

            for (int i = 0; i < productIds.Length; i++)
            {
                var product = _dbContext.Products.Find(productIds[i]);

                if (product != null)
                {
                    product.Quantity -= qtys[i];
                    _dbContext.SaveChanges();
                    updatedProducts.Add(product);
                }
            }
            return Json(updatedProducts);
        }
    }
}

