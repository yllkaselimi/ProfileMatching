using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



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
            //^we only needed this per at butonin me select n'baze t'kategorise per me filtru freelancers

            var dataContext = _context.FreelancerDetails.Include(f => f.ApplicationUser).Include(f => f.Category).Include(f => f.City);
            return View(await dataContext.ToListAsync());
        }



        //GET SUGGESTED FREELANCERS FOR A JOB BASED ON THE JOB'S CATEGORY
        public async Task<IActionResult> Suggested(int? id)
        {
            ViewData["Categories"] = _context.Categories.ToList();

            //i marrim freelancers qe e kan categoryId t njejt me kategorine qe e ka puna
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

            //qitu veq po e marrim emrin e kategorise te zgjedhne per me filtru, n'baze te id-s se kategorise
            var category = _context.Categories.Where(x => x.CategoryId == id).First();
            ViewData["FilteredCategoryName"] = category.CategoryName;

            //i kthen freelancers me qat kategori
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

            //tekstin qe e ka shkru perdoruesi n'search input, kqyrim per kolona t ndryshme t tabeles se a e permbajne
            var dataContext = _context.FreelancerDetails.Include(f => f.ApplicationUser).Include(f => f.Category).Include(f => f.City);
            var result = dataContext.Where(f => f.ApplicationUser.UserName.ToLower().Contains(teksti.ToLower())
                || f.ApplicationUser.LastName.ToLower().Contains(teksti.ToLower())
                || f.City.CityName.ToLower().Contains(teksti.ToLower())
                || f.Category.CategoryName.ToLower().Contains(teksti.ToLower()));

            return View(await result.ToListAsync());
        }
    }
}
