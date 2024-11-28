using Conversor_Monedas_Api.Repositories;
using Conversor_Monedas_Api.Services;
using Conversor_Monedas_Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Conversor_Monedas_Api.Interfaces.repositories;
using Conversor_Monedas_Api.Interfaces.services;
using Microsoft.OpenApi.Models;
using Conversor_Monedas_Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .WithOrigins("http://localhost:4200") // URL del frontend
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configuración de Entity Framework y la base de datos (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IMonedaRepository, MonedaRepository>();
builder.Services.AddScoped<IConversionRepository, ConversionRepository>();
builder.Services.AddScoped<ISuscripcionRepository, SuscripcionRepository>();

// Servicios
builder.Services.AddScoped<IConversionService, ConversionService>();
builder.Services.AddScoped<IMonedaService, MonedaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ISuscripcionService, SuscripcionService>();

// conf JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        // Configuración de eventos de JwtBearer
        options.Events = new JwtBearerEvents //endpoint
        {
            OnTokenValidated = context =>
            {
                var claimsPrincipal = context.Principal;

                // Validar que el reclamo "sub" (userId) esté presente
                if (claimsPrincipal.FindFirst("sub") == null)
                {
                    throw new SecurityTokenException("El token no contiene el ID del usuario ('sub').");
                }

                // Validar que el rol esté presente
                if (claimsPrincipal.FindFirst("role") == null)
                {
                    throw new SecurityTokenException("El token no contiene el rol del usuario ('role').");
                }

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                // Manejar errores de autenticación
                Console.WriteLine($"Authentication Failed: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });

// Configuración de Swagger para manejar JWT
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Por favor ingrese un token JWT con el formato 'Bearer {token}'",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Agregar controladores
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
});

builder.Services.AddEndpointsApiExplorer();


//builder.Services.AddAuthorization(); // Agrega servicios de autorización


var app = builder.Build();

// Configuración de la tubería de solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Conversor de Monedas API v1");
    });
}

app.UseHttpsRedirection();

//app.Use(async (context, next) =>
//{
//    if (context.Request.Method == "OPTIONS")
//    {
//        context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");
//        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
//        context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
//        context.Response.StatusCode = 204; // Sin contenido
//        return;
//    }
//    await next();
//});

// Habilitar CORS
app.UseCors("AllowAllOrigins");

app.Use(async (context, next) => //endpoint
{
    // Punto de interrupción aquí para analizar antes de la autenticación
    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
    Console.WriteLine($"Authorization Header: {authHeader}");

    if (string.IsNullOrEmpty(authHeader))
    {
        Console.WriteLine("No Authorization header found.");
    }
    else
    {
        // Validar que el formato sea correcto (Bearer <token>)
        if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();
            Console.WriteLine($"Token: {token}");
        }
        else
        {
            Console.WriteLine("Authorization header is not in Bearer format.");
        }
    }

    await next.Invoke();

    // Punto de interrupción aquí para analizar después de la autenticación/autorization
    Console.WriteLine($"Response Status Code: {context.Response.StatusCode}");

    // Verifica si el código de estado es 401 (no autorizado)
    if (context.Response.StatusCode == 401)
    {
        Console.WriteLine("Authentication failed. Invalid or missing token.");
    }
});

app.UseAuthentication();// Middleware de autenticación

// Habilitar autorización
app.UseAuthorization();// Middleware de autorización

// Mapear los controladores
app.MapControllers();

// Ejecutar la aplicación
app.Run();

