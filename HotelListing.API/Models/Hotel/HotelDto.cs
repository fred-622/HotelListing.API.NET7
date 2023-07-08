namespace HotelListing.API.Models.Hotel
{
    // added vid 34
    public class HotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        public int CountryId { get; set; }
    }
}
