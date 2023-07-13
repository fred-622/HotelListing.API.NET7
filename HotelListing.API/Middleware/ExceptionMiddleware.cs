using HotelListing.API.Exceptions;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace HotelListing.API.Middleware
{
    public class ExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        // vid 63 RequestDelegate grabs the request coming in in the pipeline and assings it to next
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        // Checks request for producing an exception
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // statusCode: 500 means our error.
                _logger.LogError(ex, $"Something Went Wrong while processing : {context.Request.Path}");
                // custom exception handler
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // write a 500 error and set details
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            var errorDetails = new ErrorDetails
            {
                ErrorType = "Failure",
                ErrorMessage = ex.Message,
            };

            switch (ex)
            {
                // looking for not found exception as difined by our NotFoundException class
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorDetails.ErrorType = "Not Found";
                    break;
                default:
                    break;
            }

            // convert the errorDetails object into a Json string
            string response = JsonConvert.SerializeObject(errorDetails);
            
            // add the status Code to the Response
            context.Response.StatusCode = (int)statusCode;

            // return the response
            return context.Response.WriteAsync(response);
        }
    }
}
