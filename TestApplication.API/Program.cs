using TestApplication.API.Container;
using TestApplication.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Force the app to listen on the desired HTTPS port when running from dotnet run / Visual Studio
builder.WebHost.UseUrls("https://localhost:7224");

// Configure External API Settings
builder.Services.Configure<ExternalApiSettings>(builder.Configuration.GetSection("ExternalApis"));

// Add HttpClient for calling external APIs
builder.Services.AddHttpClient();

// Register Custom Container (Dependency Injection, Database connection)
CustomConatiner.AddCustomContainer(builder.Services, builder.Configuration);

// Configure CORS for Angular SPA (allow all for development)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TestApplication.API v1");
        // Serve the Swagger UI at application's root (https://localhost:7224)
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
