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
    public class SalesOrderController : Controller
    {
        private readonly EcommerceDBContext _context;

        public SalesOrderController(EcommerceDBContext context)
        {
            _context = context;
        }

        // GET: SalesOrder
        public async Task<IActionResult> Index()
        {
            var ecommerceDBContext = _context.SalesOrders.Include(s => s.Customer);
            return View(await ecommerceDBContext.ToListAsync());
        }

        // GET: SalesOrder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SalesOrders == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // GET: SalesOrder/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
            return View();
        }

        // POST: SalesOrder/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderNo,OrderDate,CustomerId")] SalesOrder salesOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", salesOrder.CustomerId);
            return View(salesOrder);
        }

        // GET: SalesOrder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SalesOrders == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders.FindAsync(id);
            if (salesOrder == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", salesOrder.CustomerId);
            return View(salesOrder);
        }

        // POST: SalesOrder/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderNo,OrderDate,CustomerId")] SalesOrder salesOrder)
        {
            if (id != salesOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesOrderExists(salesOrder.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", salesOrder.CustomerId);
            return View(salesOrder);
        }

        // GET: SalesOrder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SalesOrders == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // POST: SalesOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SalesOrders == null)
            {
                return Problem("Entity set 'EcommerceDBContext.SalesOrders'  is null.");
            }
            var salesOrder = await _context.SalesOrders.FindAsync(id);
            if (salesOrder != null)
            {
                _context.SalesOrders.Remove(salesOrder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesOrderExists(int id)
        {
          return (_context.SalesOrders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
