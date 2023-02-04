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
            //id t userit qe osht logged in
            var userId = _userManager.GetUserId(HttpContext.User);

            var clientInfo = _context.ClientDetails.Include(f => f.ApplicationUser)
           .Include(f => f.City)
           .FirstOrDefault(f => f.UserId == userId);

            /*nese ndatabaz clientDetails jon empty, i bjen qe hala klienti ska shti detials per veten,
              kshtuqe t'shtin niher me shku te Create form per clientDetails para sa me shku tani te profili*/
            if (clientInfo == null)
            {
                return RedirectToAction("Create", "ClientDetails");
            }

            //t dhanat prej tabelave tjera meqe smujm mi bo return, veq i bartim me viewData
            var jobpost = _context.JobPosts.Include(f => f.Category).Where(f => f.UserId == userId).ToList();
            ViewData["JobPosts"] = jobpost;

            return View(clientInfo);
        }
        public IActionResult View(string? id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.Users.Where(x => x.Id == userId).First();

            //i bartim id te puneve qe useri has already applied for me ni viewdata
            var appliedJobs = _context.ApplicantsPerJobs.Where(x => x.UserId == userId).Select(x => x.JobPostId).ToList();
            ViewData["AppliedJobs"] = appliedJobs;

            //i bartim id te puneve qe useri has already saves for me ni viewdata
            var savedJobs = _context.SavedJobs.Where(x => x.UserId == userId).Select(x => x.JobPostId).ToList();
            ViewData["SavedJobs"] = savedJobs;

            var clientInfo = _context.ClientDetails.Include(f => f.ApplicationUser)
           .Include(f => f.City)
           .FirstOrDefault(f => f.UserId == id);

            /*nese ndatabaz clientDetails jon empty, i bjen qe hala klienti ska shti detials per veten,
              kshtuqe t'shtin niher me shku te Create form per clientDetails para sa me shku tani te profili*/
            if (clientInfo == null)
            {
                return RedirectToAction("Create", "ClientDetails");
            }

            //t dhanat prej tabelave tjera meqe smujm mi bo return, veq i bartim me viewData
            var jobpost = _context.JobPosts.Include(f => f.Category).Where(f => f.UserId == id).ToList();
            ViewData["JobPosts"] = jobpost;

            return View(clientInfo);
        }
    }
}
