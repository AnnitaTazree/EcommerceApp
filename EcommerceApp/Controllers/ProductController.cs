using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Models;
using EcommerceApp.Models.ViewModels;

namespace EcommerceApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly EcommerceDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(EcommerceDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
              return _context.Products != null ? 
                          View(await _context.Products.ToListAsync()) :
                          Problem("Entity set 'EcommerceDBContext.Products'  is null.");
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Unit,Price,Quantity,Image")] ProductView productView)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                Product product = new Product();
                product.Id = productView.Id;
                product.Name = productView.Name;
                product.Unit= productView.Unit;
                product.Price= productView.Price;
                product.Quantity= productView.Quantity;
                //Image
                string webroot = _webHostEnvironment.WebRootPath;
                string folder = "Images";
                string fileName = Path.GetFileName(productView.Image.FileName);
                string fileToWrite = Path.Combine(webroot, folder, fileName);
                //For folder
                using (var stream = new FileStream(fileToWrite, FileMode.Create))
                {
                    await productView.Image.CopyToAsync(stream);
                }

                //For DB
                using (MemoryStream ms = new MemoryStream())
                {
                    await productView.Image.CopyToAsync(ms);
                    product.Image = ms.ToArray();
                    product.ImageFile = "/" + folder + "/" + fileName;
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                msg = "Data saved successfully!!!";
                TempData["msg"] = msg;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                msg = "Product data is incomplete, Please try again!!!";
            }
            TempData["msg"] = msg;
            return RedirectToAction("Create");
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Quantity")] Product product)
        {
            
            try
            {
                // Retrieve the existing entity from the database
                var existingProduct = await _context.Products.FindAsync(product.Id);

                if (existingProduct == null)
                {
                    return NotFound();
                }

                // Update specific columns
                _context.Entry(existingProduct).Property(p => p.Name).CurrentValue = product.Name;
                _context.Entry(existingProduct).Property(p => p.Price).CurrentValue = product.Price;
                _context.Entry(existingProduct).Property(p => p.Quantity).CurrentValue = product.Quantity;

                // Save changes
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }




        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'EcommerceDBContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
