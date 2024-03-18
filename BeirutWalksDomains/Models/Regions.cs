using System.ComponentModel.DataAnnotations;

namespace BeirutWalksDomains.Models
{
    public class Regions
    {
        public Guid Id { get; set; }
        [StringLength(5, ErrorMessage = "Code shouldn't be less than 2", MinimumLength = 2)]
        public string Code { get; set; }
        [Required]
        [StringLength(100,ErrorMessage = "Name shouldn't be less than 5",MinimumLength =5)]
        public string Name { get; set; }
        public string RegionImageUrl { get; set; }
    }
}