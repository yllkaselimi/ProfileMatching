using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace ProfileMatching.Controllers
{
    public class ClientsController : Controller
    {

        private readonly DataContext _context;


        public ClientsController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Categories"] = _context.Categories.ToList();
            //^we only needed this per at butonin me select n'baze t'kategorise per me filtru freelancers

            var dataContext = _context.ClientDetails.Include(f => f.ApplicationUser).Include(f => f.City);
            return View(await dataContext.ToListAsync());
        }



       
        public async Task<IActionResult> SearchClients(string teksti)
        {
            
            ViewData["SearchInput"] = teksti;

            //tekstin qe e ka shkru perdoruesi n'search input, kqyrim per kolona t ndryshme t tabeles se a e permbajne

            var dataContext = _context.ClientDetails.Include(f => f.ApplicationUser).Include(f => f.City);
            var result = dataContext.Where(f => f.ApplicationUser.UserName.ToLower().Contains(teksti.ToLower())
                || f.ApplicationUser.LastName.ToLower().Contains(teksti.ToLower())
                || f.City.CityName.ToLower().Contains(teksti.ToLower())
                || f.Position.ToLower().Contains(teksti.ToLower())
                || f.Description.ToLower().Contains(teksti.ToLower())
                || f.CompanyName.ToLower().Contains(teksti.ToLower())); 
                


            return View(await result.ToListAsync());
        }
    }
}
