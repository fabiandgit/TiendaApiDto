using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
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
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
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

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, //Verifica que el token venga del emisor correcto
        ValidateAudience = true, //Verifica que el token esté destinado al público correcto
        ValidateLifetime = true, //Verifica que el token no esté expirado
        ValidateIssuerSigningKey = true, //Verifica que la firma del token sea válida
        ValidIssuer = jwtSettings["Issuer"], //Valor esperado del emisor (de appsettings.json)
        ValidAudience = jwtSettings["Audience"], //Valor esperado de la audiencia (de appsettings.json)
        IssuerSigningKey = new SymmetricSecurityKey(key) //Clave secreta para validar la firma del token
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // origen de tu frontend
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthentication(); //  Verifica el token
app.UseAuthorization();  //  Aplica reglas de acceso
app.MapControllers();

app.Run();