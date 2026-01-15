// Controllers/PackagesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Workshop04.Data.Data;
using Workshop04.Data.Models;
using Workshop04.Data.Services;
using Workshop04.ViewModels;

namespace Workshop04.Controllers
{
    public class PackagesController : Controller
    {
        private readonly TravelExpertsContext _context;
        private readonly UserManager<Customer> _userManager;

        public PackagesController(TravelExpertsContext context, UserManager<Customer> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Packages - Anyone can view
        public async Task<IActionResult> Index()
        {
            List<Package> packages = new List<Package>();
            try
            {
                packages = await _context.Packages
                    .Include(p => p.PackagesProductsSuppliers)
                        .ThenInclude(pps => pps.ProductsSupplier)
                            .ThenInclude(ps => ps.Product)
                    .ToListAsync();
            }
            catch
            {
                TempData["Message"] = "No packages available at the moment. Please check back later.";
                TempData["IsError"] = true;
            }
            return View(packages);
        }

        // GET: Packages/Details/5 - Anyone can view
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Package? package = null;

            try
            {
                package = await _context.Packages
                    .Include(p => p.PackagesProductsSuppliers)
                        .ThenInclude(pps => pps.ProductsSupplier)
                            .ThenInclude(ps => ps.Product)
                    .FirstOrDefaultAsync(m => m.PackageId == id);

                if (package == null)
                {
                    return NotFound();
                }
            }
            catch
            {
                TempData["Message"] = "No packages available at the moment. Please check back later.";
                TempData["IsError"] = true;
                return RedirectToAction("Index", new List<Package>()); //Go to package page and display error if no database connection
            }

            return View(package);
        }

        // GET: Packages/Purchase/5 - Requires login
        [Authorize]
        public async Task<IActionResult> Purchase(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PurchasePackageViewModel? viewModel = null;

            try
            {
                var package = await _context.Packages
                    .Include(p => p.PackagesProductsSuppliers)
                        .ThenInclude(pps => pps.ProductsSupplier)
                            .ThenInclude(ps => ps.Product)
                        .FirstOrDefaultAsync(m => m.PackageId == id);

                if (package == null)
                {
                    return NotFound();
                }

                // NEW: Check if package is active
                if (!package.IsActive)
                {
                    TempData["ErrorMessage"] = $"Sorry, the {package.PkgName} package is no longer available. The travel dates have passed.";
                    return RedirectToAction("Details", new { id = package.PackageId });
                }

                // Get current logged-in customer
                var customer = await _userManager.GetUserAsync(User);

                if (customer == null)
                {
                    return Redirect("/Identity/Account/Login");
                }

                viewModel = new PurchasePackageViewModel
                {
                    Package = package,
                    Customer = customer,
                    PackageId = package.PackageId,
                    CustomerId = customer.CustomerId,
                    NumberOfTravelers = 1, // Default to 1 traveler
                    TotalAmount = package.PkgBasePrice, // Will be recalculated based on travelers
                    AvailableCredit = customer.CreditBalance
                };
            }
            catch
            {
                TempData["Message"] = "No packages available at the moment. Please check back later.";
                TempData["IsError"] = true;
                return RedirectToAction("Index", new List<Package>()); //Go to package page and display error if no database connection
            }
            return View(viewModel);
        }

        // POST: Packages/Purchase
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Purchase(PurchasePackageViewModel model)
        {
            try
            {
                // Get current logged-in customer
                var customer = await _userManager.GetUserAsync(User);

                if (customer == null)
                {
                    return Redirect("/Identity/Account/Login");
                }

                // Validate number of travelers
                if (model.NumberOfTravelers < 1 || model.NumberOfTravelers > 20)
                {
                    ModelState.AddModelError("NumberOfTravelers", "Number of travelers must be between 1 and 20.");
                }

                if (!ModelState.IsValid)
                {
                    // Reload package data
                    model.Package = await _context.Packages
                        .Include(p => p.PackagesProductsSuppliers)
                            .ThenInclude(pps => pps.ProductsSupplier)
                                .ThenInclude(ps => ps.Product)
                        .FirstOrDefaultAsync(m => m.PackageId == model.PackageId);

                    model.Customer = customer;
                    model.AvailableCredit = customer.CreditBalance;

                    // Recalculate total amount
                    if (model.Package != null)
                    {
                        model.TotalAmount = model.Package.PkgBasePrice * model.NumberOfTravelers;
                    }

                    return View(model);
                }

                var package = await _context.Packages
                    .FirstOrDefaultAsync(p => p.PackageId == model.PackageId);

                if (package == null)
                {
                    return NotFound();
                }

                // NEW: Check if package is still active before processing payment
                if (!package.IsActive)
                {
                    TempData["ErrorMessage"] = $"Sorry, the {package.PkgName} package is no longer available. The travel dates have passed.";
                    return RedirectToAction("Index");
                }

                // Calculate total cost based on number of travelers
                decimal totalCost = package.PkgBasePrice * model.NumberOfTravelers;
                model.TotalAmount = totalCost;

                // Check if customer has sufficient credit
                if (customer.CreditBalance < totalCost)
                {
                    ModelState.AddModelError("",
                        $"Insufficient credit balance. You need ${totalCost:F2} for {model.NumberOfTravelers} traveler(s) but only have ${customer.CreditBalance:F2}");

                    model.Package = await _context.Packages
                        .Include(p => p.PackagesProductsSuppliers)
                            .ThenInclude(pps => pps.ProductsSupplier)
                                .ThenInclude(ps => ps.Product)
                        .FirstOrDefaultAsync(m => m.PackageId == model.PackageId);
                    model.Customer = customer;
                    model.AvailableCredit = customer.CreditBalance;

                    return View(model);
                }

                // Create booking
                var booking = new Booking
                {
                    BookingDate = DateTime.Now,
                    BookingNo = GenerateBookingNumber(),
                    TravelerCount = model.NumberOfTravelers,
                    CustomerId = customer.Id,
                    PackageId = package.PackageId,
                    TripTypeId = "L" // L for Leisure
                };

                // Deduct from customer's credit balance
                customer.CreditBalance -= totalCost;

                _context.Bookings.Add(booking);
                await _userManager.UpdateAsync(customer);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Package purchased successfully for {model.NumberOfTravelers} traveler(s)!";
                return RedirectToAction("Confirmation", new { id = booking.BookingId });
            }
            catch
            {
                TempData["PayErrMessage"] = "An error occurred while processing your request. Please try again later.";
                TempData["IsError"] = true;
                return RedirectToAction("Index", new List<Package>()); //Go to package page and display error if no database connection
            }
        }

        // GET: Packages/Confirmation/5
        [Authorize]
        public async Task<IActionResult> Confirmation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _userManager.GetUserAsync(User);

            if (customer == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            var booking = await _context.Bookings
                .Include(b => b.Package)
                    .ThenInclude(p => p.PackagesProductsSuppliers)
                        .ThenInclude(pps => pps.ProductsSupplier)
                            .ThenInclude(ps => ps.Product)
                .Include(b => b.Customer)
               .FirstOrDefaultAsync(m => m.BookingId == id && m.CustomerId == customer.Id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Packages/MyBookings
        [Authorize]
        public async Task<IActionResult> MyBookings()
        {
            List<Booking> bookings = new List<Booking>();
            try
            {
                var customer = await _userManager.GetUserAsync(User);

                if (customer == null)
                {
                    return Redirect("/Identity/Account/Login");
                }

                bookings = await _context.Bookings
                    .Include(b => b.Package)
                        .ThenInclude(p => p.PackagesProductsSuppliers)
                            .ThenInclude(pps => pps.ProductsSupplier)
                                .ThenInclude(ps => ps.Product)
                    .Where(b => b.CustomerId == customer.Id)
                    .OrderByDescending(b => b.BookingDate)
                    .ToListAsync();
            }
            catch
            {
                TempData["Message"] = "Unable to check bookings at the moment. Please check back later.";
                TempData["IsError"] = true;
            }

            return View(bookings);
        }

        // POST: Packages/CancelBooking/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                var customer = await _userManager.GetUserAsync(User);

                if (customer == null)
                {
                    return Redirect("/Identity/Account/Login");
                }

                var booking = await _context.Bookings
                    .Include(b => b.Package)
                    .FirstOrDefaultAsync(b => b.BookingId == id && b.CustomerId == customer.Id);

                if (booking == null)
                {
                    return NotFound();
                }

                // Calculate refund amount based on number of travelers
                var refundAmount = (booking.Package?.PkgBasePrice ?? 0) * (decimal)(booking.TravelerCount ?? 1);
                customer.CreditBalance += refundAmount;

                // Delete the booking
                _context.Bookings.Remove(booking);
                await _userManager.UpdateAsync(customer);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Booking #{booking.BookingNo} has been cancelled. ${refundAmount:N2} has been refunded to your wallet.";
            }
            catch
            {
                TempData["CencelBookErrMessage"] = "Unable to cancel booking at the moment. Please try again later.";
                TempData["IsError"] = true;
            }
            return RedirectToAction("MyBookings");
        }

        // Helper method to generate unique booking number
        private string GenerateBookingNumber()
        {
            return $"BK{DateTime.Now:yyyyMMddHHmmss}";
        }
    }
}