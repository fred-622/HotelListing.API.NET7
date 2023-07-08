using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Country
{
    // add from vid 35, abstract class canot be intantiated used Inheritance
    public abstract class BaseCountryDto
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
