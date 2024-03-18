using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeirutWalksDomains.Dto
{
    public class WalksDto
    {
        public Guid Id { get; set; }
       
        public string Name { get; set; }

        public string Description { get; set; }
        public double LengthInKM { get; set; }
        public string WalkImageURL { get; set; }
        public Guid RegionsId { get; set; }
        public Guid DifficultyID { get; set; }
    }
}
