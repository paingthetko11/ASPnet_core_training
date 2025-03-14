using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WebApplication1.Models;


static bool IsOriginAllowed(string origin)
{
    Uri uri = new(origin);
    bool isAllowed = uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase);
    return isAllowed;
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Enable Cors
_ = builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowedCorsOrigins",
        builder =>
        {
            
            _ = builder
            .SetIsOriginAllowed(IsOriginAllowed)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference("/docs/scalar");
}

app.UseHttpsRedirection();

app.UseCors("AllowedCorsOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
