using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileMatching.Models
{
    public class JobPost
    {
        [Key]
        public int JobPostId { get; set; }
        public string JobPostName { get; set; }
        public int JobPostBudget { get; set; }
        public DateTime JobLength { get; set; }
        public string JobPostDescription { get; set; }
        public DateTime JobApplicationDeadline { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }




    }
}
