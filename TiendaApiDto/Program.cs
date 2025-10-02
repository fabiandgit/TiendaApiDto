using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using TiendaApiDto.Data;
using TiendaApiDto.Mappers;
using TiendaApiDto.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<TiendaApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddAutoMapper(typeof(TiendaProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Tienda API",
        Version = "v1",
        Description = "API para gestión de productos en una tienda. Incluye operaciones CRUD con y sin AutoMapper.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Tu Nombre",
            Email = "tunombre@correo.com"
        }
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();