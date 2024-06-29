using Book_Management_System.Data;
using Book_Management_System.Data.Models;
using Book_Management_System.Interfaces.Data;
using Book_Management_System.Interfaces.Services;
using Book_Management_System.Services;
using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Data.Repositories.DataBase;
using Book_Management_System.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddDbRepositories<BMSContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BMSContext>(options =>
{
    options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("MyDatabase"));
});

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
