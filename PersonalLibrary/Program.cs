using Microsoft.EntityFrameworkCore;
using PersonalLibrary.Core;
using PersonalLibrary.Core.Contracts.Repositories;
using PersonalLibrary.Infrastructure;
using PersonalLibrary.Infrastructure.Repositories;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Set the ReferenceHandler to IgnoreCycles to prevent circular references
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

    // Optionally, configure other JSON options
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;

    //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//registering services
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

//adding automapper to automatically map entities
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<LibraryDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
