using System.ComponentModel.DataAnnotations;

namespace ProfileMatching.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
