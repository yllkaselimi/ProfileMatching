using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ProfileMatching.Controllers
{
    public class ClientHomePage : Controller
    {

        private readonly DataContext _context;


        public ClientHomePage(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Categories"] = _context.Categories.ToList();
            //^we only needed this per at butonin me select n'baze t'kategorise per me filtru freelancers

            var dataContext = _context.FreelancerDetails.Include(f => f.ApplicationUser).Include(f => f.Category).Include(f => f.City).OrderByDescending(j => j.FreelancerDetailsId).Take(3);

            var slider = _context.Sliders.ToList();
            ViewData["Sliders"] = slider;
            return View(await dataContext.ToListAsync());

        }
    }
}