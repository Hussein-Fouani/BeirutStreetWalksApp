using System.ComponentModel.DataAnnotations;

namespace BeirutWalksDomains.Models
{
    public class Difficulty
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(20,ErrorMessage ="Difficulty must be less than 20", MinimumLength = 5)]
        public string Name { get; set; }
    }
}