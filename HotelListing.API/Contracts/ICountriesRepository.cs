using HotelListing.API.Data;

namespace HotelListing.API.Contracts
{
    // Interface for Country
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetails(int id);
    }



}
