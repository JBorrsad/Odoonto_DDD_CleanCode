using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Odoonto.Infrastructure.Configuration.Firebase;
using Odoonto.Infrastructure.InversionOfControl.Inyectors;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;

// Configure Firebase
var firebaseSection = configuration.GetSection("Firebase");
if (firebaseSection.Exists())
{
    string apiKey = firebaseSection["ApiKey"] ?? string.Empty;
    string credentialsPath = firebaseSection["CredentialsPath"] ?? "firebase-credentials.json";
    var credentialsFullPath = Path.Combine(builder.Environment.ContentRootPath, credentialsPath);

    try
    {
        FirebaseConfiguration.Instance.Initialize(apiKey, credentialsFullPath, null);
        builder.Services.AddSingleton(FirebaseConfiguration.Instance);
        builder.Services.AddSingleton(FirebaseConfiguration.Instance.GetFirestoreDb());
        Console.WriteLine($"Firebase configured successfully with credentials at {credentialsFullPath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Firebase configuration failed: {ex.Message}");
        Console.WriteLine("The application will continue, but Firebase functionalities will not be available.");
    }
}

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
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
        Console.WriteLine($"CORS configured with {allowedOrigins.Length} allowed origins");
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

    // Set the comments path for the Swagger JSON and UI
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Register application services
builder.Services.AddRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configurar el manejo de errores
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        await context.Response.WriteAsJsonAsync(new
        {
            error = "Ha ocurrido un error al procesar la solicitud",
            detail = app.Environment.IsDevelopment() ? exception?.Message : null
        });
    });
});

app.UseHttpsRedirection();

// Use CORS
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();