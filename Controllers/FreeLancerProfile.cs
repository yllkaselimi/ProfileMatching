using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System;

namespace ProfileMatching.Controllers
{
    public class FreeLancerProfile : Controller
    {

        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FreeLancerProfile(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var freeLancerInfo = _context.FreelancerDetails.Include(f => f.ApplicationUser)
           .Include(f => f.City)
           .Include(f => f.Category)
           .FirstOrDefault(f => f.UserId == userId);

            var projekti = _context.Projects.Include(f => f.Category).Where(f => f.UserId == userId).ToList();
            ViewData["Projects"] = projekti;

            var freelancerexperience = _context.FreelancerExperiences.Where(f => f.UserId == userId).ToList();
            ViewData["FreeLancerExperiences"] = freelancerexperience;

            var freelancereducation = _context.FreelancerEducations.Where(f => f.UserId == userId).ToList();
            ViewData["FreeLancerEducations"] = freelancereducation;


            return View(freeLancerInfo);
            

        }
    }
}
