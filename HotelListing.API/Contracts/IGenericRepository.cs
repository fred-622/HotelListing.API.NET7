namespace HotelListing.API.Contracts
{
    // vid 36, the interface is responsible for communicating with the Db
    public interface IGenericRepository<T> where T : class
    {
        // Get, one record of our data models (Dto)
        Task<T> GetAsync(int? id);

        //Get by id, all records from endpoint
        Task<List<T>> GetAllAsync();

        // Post, Add to Db
        Task<T> AddAsync(T entity);
        
        // Delete from Db
        Task DeleteAsync(int id);

        // Put, Update Db
        Task<T> UpdateAsync(T entity);

        // DbContext check if record exist
        Task<bool> Exists(int id);
    }



}
