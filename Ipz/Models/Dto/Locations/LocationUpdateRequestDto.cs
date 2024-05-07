using System.ComponentModel.DataAnnotations;

namespace Ipz_server.Models.Dto.Locations
{
    public class LocationUpdateRequestDto
    {
        [Required]
        public Guid LocationId { get; set; }
        [MinLength(2)]
        public string? Country { get; set; }
        [MinLength(2)]
        public string? City { get; set; }
        [MinLength(5)]
        public string? Street { get; set; }
    }
}
