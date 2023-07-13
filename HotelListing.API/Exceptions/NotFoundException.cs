namespace HotelListing.API.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        // vid 63 custom exception
        public NotFoundException(string name, object key) : base($"{name} ({key}) was not found")
        {

        }
    }
}
