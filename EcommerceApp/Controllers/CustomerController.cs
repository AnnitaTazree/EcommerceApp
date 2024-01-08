using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Models;

namespace EcommerceApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly EcommerceDBContext _dbContext;

        public CustomerController(EcommerceDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Customer
        //public async Task<IActionResult> Index()
        //{
        //      return _context.Customers != null ? 
        //                  View(await _context.Customers.ToListAsync()) :
        //                  Problem("Entity set 'EcommerceDBContext.Customers'  is null.");
        //}
        public IActionResult Index()
        {
            return View(_dbContext.AppUsers.ToList());
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _dbContext.Customers == null)
            {
                return NotFound();
            }

            var customer = await _dbContext.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Mobile,Address")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(customer);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _dbContext.Customers == null)
            {
                return NotFound();
            }

            var customer = await _dbContext.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Mobile,Address")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(customer);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _dbContext.Customers == null)
            {
                return NotFound();
            }

            var customer = await _dbContext.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_dbContext.Customers == null)
            {
                return Problem("Entity set 'EcommerceDBContext.Customers'  is null.");
            }
            var customer = await _dbContext.Customers.FindAsync(id);
            if (customer != null)
            {
                _dbContext.Customers.Remove(customer);
            }
            
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return (_dbContext.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
