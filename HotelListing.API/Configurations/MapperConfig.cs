using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using HotelListing.API.Models.Hotel;

// added in vid 33
namespace HotelListing.API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            // Mapper basically converts (maps) Country to CreateCountryDto
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            // vid 34
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            // vid 35
            CreateMap<Country, UpdateCountryDto>().ReverseMap();
            // vid 34
            CreateMap<Hotel, HotelDto>().ReverseMap();

        }
    }
}
