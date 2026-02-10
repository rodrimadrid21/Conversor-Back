using Conversor_Monedas_Api.Data;
using Conversor_Monedas_Api.Interfaces.repositories;
using Conversor_Monedas_Api.Interfaces.services;
using Conversor_Monedas_Api.Repositories;
using Conversor_Monedas_Api.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;              // Encoding


var builder = WebApplication.CreateBuilder(args);

// CORS (Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(options =>//addDbContext registra DbContext como scooped
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

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//define bearer jwt
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;//evita q .net remapee claims

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,//exige firma valida
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

            // - seguridad, cualquier iss y aud
            ValidateIssuer = false,
            ValidateAudience = false,

            ClockSkew = TimeSpan.Zero
        };
    });

// habilita autorizacion
builder.Services.AddAuthorization();

// Controllers (con Authorize global)
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
});

// permite que swagger reconozca los endpoints
builder.Services.AddEndpointsApiExplorer();

// Swagger + Bearer (agrega el boton Authorize en swagger)
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Conversor de Monedas API", 
        Version = "v1" 
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT con formato: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
//construye el host y la app
var app = builder.Build();

//pipeline de middlewares
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Conversor de Monedas API v1");
    });
}

//middlewares
app.UseHttpsRedirection();//redirige http a https

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();//mapea rutas a controllers

app.Run();//inicia la app
