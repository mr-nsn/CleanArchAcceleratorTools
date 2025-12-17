using CleanArchAcceleratorTools.Examples.Configurations.Extensions;
using CleanArchAcceleratorTools.Examples.Infrastructure.IoC;
using Newtonsoft.Json;
using CleanArchAcceleratorTools.Controller.Filters;

var builder = WebApplication.CreateBuilder(args);

// CORS configuration
builder.Services.AddCors();

// Add services to the container.
builder.Services.RegisterServices(builder.Configuration);

// Add Mapster mappings
builder.Services.RegisterMappers();

// Controllers and Newtonsoft JSON configuration
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ExceptionFilter)); // Register ExceptionFilter globally
}).AddNewtonsoftJson(config =>
{
    config.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    config.SerializerSettings.Formatting = Formatting.Indented;
    config.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
});

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Swagger configuration
    app.UseSwagger();
    app.UseSwaggerUI();

    // CORS configuration
    app.UseCors(c =>
    {
        c.AllowAnyHeader();
        c.AllowAnyMethod();
        c.AllowAnyOrigin();
        c.WithExposedHeaders("Content-Disposition");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.SeedContext();

app.Run();
