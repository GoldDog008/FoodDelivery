using System.ComponentModel.DataAnnotations;

namespace Ipz_server.Models.Dto.Locations
{
    public class LocationCreateRequestDto
    {
        [MinLength(2)]
        public string? Country { get; set; }
        [MinLength(2)]
        public string? City { get; set; }
        public string? Street { get; set; }
    }
}
