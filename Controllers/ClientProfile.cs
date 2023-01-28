using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace ProfileMatching.Controllers
{
    public class ClientProfile : Controller
    {

        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ClientProfile(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var clientInfo = _context.ClientDetails.Include(f => f.ApplicationUser)
           .Include(f => f.City)
           .FirstOrDefault(f => f.UserId == userId);

            var jobpost = _context.JobPosts.Include(f => f.Category).Where(f => f.UserId == userId).ToList();
            ViewData["JobPosts"] = jobpost;
            
            return View(clientInfo);
        }
    }
}
