using Microsoft.AspNetCore.Mvc;
using ASP.NETCoreIdentityCustom.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProfileMatching.Models;



namespace ProfileMatching.Controllers
{
    public class FreelancersController : Controller
    {

        private readonly DataContext _context;


        public FreelancersController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Categories"] = _context.Categories.ToList();

            var dataContext = _context.FreelancerDetails.Include(f => f.ApplicationUser).Include(f => f.Category).Include(f => f.City);
            return View(await dataContext.ToListAsync());
        }

        public async Task<IActionResult> Suggested(int? id)
        {
            ViewData["Categories"] = _context.Categories.ToList();

            var freelancers = _context.FreelancerDetails
                .Include(f => f.ApplicationUser)
                .Include(f => f.Category).Include(f => f.City)
                .Where(f => f.Category.CategoryId == id);

            if (freelancers == null)
            {
                return NotFound();
            }
            return View(await freelancers.ToListAsync());
        }

        public async Task<IActionResult> Filtered(int? id)
        {
            ViewData["Categories"] = _context.Categories.ToList();

            var category = _context.Categories.Where(x => x.CategoryId == id).First();
            ViewData["FilteredCategoryName"] = category.CategoryName;

            var freelancers = _context.FreelancerDetails
                .Include(f => f.ApplicationUser)
                .Include(f => f.Category).Include(f => f.City)
                .Where(f => f.Category.CategoryId == id);

            if (freelancers == null)
            {
                return NotFound();
            }
            return View(await freelancers.ToListAsync());
        }

        public async Task<IActionResult> SearchFreelancers(string teksti)
        {
            ViewData["Categories"] = _context.Categories.ToList();

            ViewData["SearchInput"] = teksti;

            var dataContext = _context.FreelancerDetails.Include(f => f.ApplicationUser).Include(f => f.Category).Include(f => f.City);
            var result = dataContext.Where(f => f.ApplicationUser.UserName.ToLower().Contains(teksti.ToLower())
                || f.ApplicationUser.LastName.ToLower().Contains(teksti.ToLower())
                || f.City.CityName.ToLower().Contains(teksti.ToLower())
                || f.Category.CategoryName.ToLower().Contains(teksti.ToLower()));

            return View(await result.ToListAsync());
        }
    }
}
