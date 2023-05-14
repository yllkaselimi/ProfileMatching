using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NETCoreIdentityCustom.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMatching.Models;

namespace ProfileMatching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantsPerJobsController : ControllerBase
    {
        private readonly DataContext _context;

        public ApplicantsPerJobsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ApplicantsPerJobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicantsPerJob>>> GetApplicantsPerJobs()
        {
          if (_context.ApplicantsPerJobs == null)
          {
              return NotFound();
          }
            return await _context.ApplicantsPerJobs.ToListAsync();
        }

        // GET: api/ApplicantsPerJobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicantsPerJob>> GetApplicantsPerJob(int id)
        {
          if (_context.ApplicantsPerJobs == null)
          {
              return NotFound();
          }
            var applicantsPerJob = await _context.ApplicantsPerJobs.FindAsync(id);

            if (applicantsPerJob == null)
            {
                return NotFound();
            }

            return applicantsPerJob;
        }

        // GET: api/ApplicantsPerJobs/GetJobpostsByClientId/5
        [HttpGet("GetJobpostsByClientId/{clientId}")]
        public async Task<ActionResult<IEnumerable<JobPost>>> GetJobpostsByClientId(string clientId)
        {
            var jobPosts = await _context.JobPosts
                .Where(j => j.UserId == clientId)
                .ToListAsync();

            if (jobPosts == null)
            {
                return NotFound();
            }

            return jobPosts;
        }

        // GET: api/ApplicantsPerJobs/GetHiredApplicantsForJobPost/5
        [HttpGet("GetHiredApplicantsForJobPost/{jobPostId}")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetHiredApplicantsForJobPost(int jobPostId)
        {
            var hiredApplicants = await _context.ApplicantsPerJobs
                .Where(a => a.JobPostId == jobPostId)
                .Where(a => a.HiredStatus == true)
                .Select(a => a.ApplicationUser) 
                .ToListAsync();

            if (hiredApplicants == null)
            {
                return NotFound();
            }

            return hiredApplicants;
        }

        // PUT: api/ApplicantsPerJobs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicantsPerJob(int id, ApplicantsPerJob applicantsPerJob)
        {
            if (id != applicantsPerJob.ApplicantPerJobId)
            {
                return BadRequest();
            }

            _context.Entry(applicantsPerJob).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicantsPerJobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApplicantsPerJobs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApplicantsPerJob>> PostApplicantsPerJob(ApplicantsPerJob applicantsPerJob)
        {
          if (_context.ApplicantsPerJobs == null)
          {
              return Problem("Entity set 'DataContext.ApplicantsPerJobs'  is null.");
          }
            _context.ApplicantsPerJobs.Add(applicantsPerJob);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApplicantsPerJob", new { id = applicantsPerJob.ApplicantPerJobId }, applicantsPerJob);
        }

        // DELETE: api/ApplicantsPerJobs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicantsPerJob(int id)
        {
            if (_context.ApplicantsPerJobs == null)
            {
                return NotFound();
            }
            var applicantsPerJob = await _context.ApplicantsPerJobs.FindAsync(id);
            if (applicantsPerJob == null)
            {
                return NotFound();
            }

            _context.ApplicantsPerJobs.Remove(applicantsPerJob);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplicantsPerJobExists(int id)
        {
            return (_context.ApplicantsPerJobs?.Any(e => e.ApplicantPerJobId == id)).GetValueOrDefault();
        }
    }
}
