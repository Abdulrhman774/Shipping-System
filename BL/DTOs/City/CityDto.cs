using BL.DTOs.Base;

namespace BL.DTOs.City
{
    public class CityDto : BaseDto
    {
        public string? CityAname { get; set; }
        public string? CityEname { get; set; }
        public Guid CountryId { get; set; }
    }
}
