using System.ComponentModel.DataAnnotations;

namespace ProfileMatching.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}
