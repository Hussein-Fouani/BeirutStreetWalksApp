using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeirutWalksDomains.Dto
{
    public class ImageUploadRequestDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
