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
            var dataContext = _context.FreelancerDetails.Include(f => f.ApplicationUser).Include(f => f.Category).Include(f => f.City);
            return View(await dataContext.ToListAsync());
        }

        public async Task<IActionResult> Suggested(int? id)
        {
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
    }
}
