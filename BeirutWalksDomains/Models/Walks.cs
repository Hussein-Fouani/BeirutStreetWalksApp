using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeirutWalksDomains.Models
{
    public class Walks
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(100,ErrorMessage ="Name shouldn't be less than 5",MinimumLength =5)]
        public string Name { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Description shouldn't be less than 15", MinimumLength = 5)]
        public string Description { get; set; }
        [Required]
        public double LengthInKM { get; set; }
        [DataType(DataType.ImageUrl)]
        public string WalkImageURL { get; set; }
        [ForeignKey("RegionsId")]
        public Guid RegionsId { get; set; }
        [ForeignKey("DifficultyID")]
        public Guid DifficultyID { get; set; }
        public Regions Regions;
        public Difficulty Difficulty;


    }
}
