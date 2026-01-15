using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Workshop04.Data.Data;
using Workshop04.Data.Data.DTO;
using Workshop04.Data.Services;
using Workshop04.Models;

namespace Workshop04.Controllers
{
    public class HomeController(TravelExpertsContext context, ILogger<HomeController> logger) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private TravelExpertsContext _context = context;

        //Displays the view for the home page
        public IActionResult Index()
        {
            List<PackageDTO> packageDTOs = new List<PackageDTO>();
            try
            {
                //Get the featured packages from database to display
                packageDTOs = PackageService.GetTop3ValidPackageDTOs(context);
            }
            catch
            {
                TempData["Message"] = "No packages available at the moment. Please check back later.";
                TempData["IsError"] = true;
            }

            return View(packageDTOs);
        }

        //Display the view for contacts
        public IActionResult Contacts()
        {
            List<AgencyDTO> agencyDTOs = new List<AgencyDTO>();
            try
            {
                //Get the list of agencies from database to display
                agencyDTOs = AgencyService.GetAllAgencyDTOs(context);
            }
            catch
            {
                TempData["Message"] = "No locations available at the moment. Please check back later.";
                TempData["IsError"] = true;
            }
            return View(agencyDTOs);

        }

        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
