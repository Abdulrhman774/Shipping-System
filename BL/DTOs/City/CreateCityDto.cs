namespace BL.DTOs.City
{
    public class CreateCityDto
    {
        public string? CityAname { get; set; }
        public string? CityEname { get; set; }
        public Guid CountryId { get; set; }
    }
}
