using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Odoonto.Infrastructure.Configuration.Firebase;
using Odoonto.Infrastructure.InversionOfControl;
using Odoonto.UI.Server.Extensions;
using Odoonto.Infrastructure.ExceptionsHandler.Extensions;
using Odoonto.Infrastructure.Authentication.Extensions;
using Odoonto.Infrastructure.Logging.Extensions;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;

// Guardar la ruta de contenido en la configuración
var contentRootPath = builder.Environment.ContentRootPath;
configuration["ContentRootPath"] = contentRootPath;

// Registrar servicios de la aplicación
builder.Services.AddApplicationServices(configuration);

// Configurar autenticación con JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // La validación del token se hace en el middleware de Firebase
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = true;
    });

// Add controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Configure CORS
var corsSection = configuration.GetSection("Cors");
if (corsSection.Exists())
{
    var allowedOrigins = corsSection.GetSection("AllowedOrigins").Get<string[]>();
    if (allowedOrigins != null && allowedOrigins.Length > 0)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });
        Console.WriteLine($"CORS configurado con {allowedOrigins.Length} orígenes permitidos");
    }
}

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Odoonto API",
        Version = "v1",
        Description = "API REST para la gestión de consultas odontológicas",
        Contact = new OpenApiContact
        {
            Name = "Equipo Odoonto",
            Email = "contacto@odoonto.com",
            Url = new Uri("https://odoonto.com/contact")
        }
    });

    // Configurar para incluir comentarios XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Configurar autenticación para Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline

// 1. Primero el middleware de excepciones para capturar errores en los middleware subsecuentes
app.UseGlobalExceptionHandler();

// 2. Después el middleware de logging para registrar todas las solicitudes 
app.UseRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Odoonto API v1");
        c.RoutePrefix = string.Empty; // Para servir la UI de Swagger en la raíz
    });
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("CorsPolicy");

// 3. Middleware de autenticación antes de autorización
app.UseFirebaseAuthentication();

// 4. Middleware de autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();