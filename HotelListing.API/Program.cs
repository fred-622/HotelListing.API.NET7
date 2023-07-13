using HotelListing.API.Configurations;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Middleware;
using HotelListing.API.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// add vid 16
var connectionString = builder.Configuration.GetConnectionString("HoteListingDbConnectionString");
builder.Services.AddDbContext<HotelListingDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// vid 49 IdentityUser has all login info plus more, dont modify
builder.Services.AddIdentityCore<ApiUser>()
    .AddRoles<IdentityRole>()
    // from vid 58
    .AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("HotelListingApi")
    .AddEntityFrameworkStores<HotelListingDbContext>()
    // from vid 58
    .AddDefaultTokenProviders();


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

// add from vid 33
builder.Services.AddAutoMapper(typeof(MapperConfig));

// add from vid 37
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();

// add vid 43
builder.Services.AddScoped<IHotelsRepository, HotelsRepository>();

// add vid 52
builder.Services.AddScoped<IAuthManager, AuthManager>();

// add vid 55 JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// vid 63
app.UseMiddleware<ExceptionMiddleware>();

//Add vid 12 auto logs created
app.UseSerilogRequestLogging();


app.UseHttpsRedirection();


// add vid 10
app.UseCors("AllowAll");

//add vid57
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
