using System.Reflection;
using LMS.infra;
using LMS.App;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication2();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var uploadPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot/uploads");
if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

var app = builder.Build();
await app.InitializeDatabaseAsync();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
