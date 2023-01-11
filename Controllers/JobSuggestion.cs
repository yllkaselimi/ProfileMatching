using Microsoft.AspNetCore.Mvc;
using ASP.NETCoreIdentityCustom.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using ProfileMatching.Models;
using Microsoft.AspNetCore.Identity;

namespace ProfileMatching.Controllers
{
    public class JobSuggestion : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public JobSuggestion(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> ListMatchingJobPosts()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound();
            }
            var matchingJobPosts = await _context.JobPosts
                .Join(_context.FreelancerDetails, jp => jp.CategoryId, fd => fd.CategoryId, (jp, fd) => new { JobPost = jp, FreelancerDetail = fd })
                .Join(_context.Users, jpfd => jpfd.FreelancerDetail.UserId, u => u.Id, (jpfd, u) => new { jpfd.JobPost, User = u })
                .Where(data => data.User.Id == user.Id)
                .Select(data => data.JobPost)
                .ToListAsync();
            if (matchingJobPosts == null)
            {
                return NotFound();
            }
            return View(matchingJobPosts);
        }
    }
    }






