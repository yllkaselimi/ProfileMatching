using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASP.NETCoreIdentityCustom.Areas.Identity.Data;


namespace ProfileMatching.Models
{
    public class Rating
    {
        public int RatingID { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public int RatingStars { get; set; }

        public String RatingComment { get; set; }
    }
}
