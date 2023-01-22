using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

string defalutConnString = configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddDbContext<BrandNewDay.Bank.Web.API.Models.BrandNewDayContext>(options =>
{
    //In my real work. I use secret storage to store username and password for development environment
    var connString = $"{defalutConnString}";
    options.UseSqlServer(connString);

});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
