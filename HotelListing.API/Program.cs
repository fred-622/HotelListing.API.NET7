using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// add vid 16
var connectionString = builder.Configuration.GetConnectionString("HoteListingDbConnectionString");
builder.Services.AddDbContext<HotelListingDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add vid 10
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod());
});

// Add vid 11
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Add vid 12 auto logs created
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

// add vid 10
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
