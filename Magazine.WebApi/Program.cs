using Magazine.Core.Data;
using Magazine.Core.Services;
using Magazine.WebApi;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
var builder = WebApplication.CreateBuilder(args);


string connectionString = "Data Source=/prog/data/database.db";

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(connectionString));
Console.WriteLine($"Using SQLite database at:", connectionString);
builder.Services.AddScoped<IProductService, DataBaseProductService>();

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var basePath = AppContext.BaseDirectory;
    var xmlPath = Path.Combine(basePath, "Magazine.WebApi.xml");
    options.IncludeXmlComments(xmlPath);
}
);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()  
                  .AllowAnyMethod()   
                  .AllowAnyHeader(); 
        });
});


//builder.Services.AddSingleton<IProductService, ProductService>();

var app = builder.Build();
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}