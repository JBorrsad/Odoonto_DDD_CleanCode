using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Odoonto.Infrastructure.InversionOfControl.Inyectors;
using System;
using System.IO;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configuración Firebase
var firebaseApiKey = builder.Configuration["Firebase:ApiKey"];
var firebaseCredentialsPath = builder.Configuration["Firebase:CredentialsPath"];
builder.Services.AddFirebaseServices(firebaseApiKey, firebaseCredentialsPath);

// Agregar servicios al contenedor
builder.Services.AddControllers();

// Configurar Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Odoonto API",
        Version = "v1",
        Description = "API REST para el sistema de gestión odontológica Odoonto",
        Contact = new OpenApiContact
        {
            Name = "Equipo de Desarrollo",
            Email = "soporte@odoonto.com"
        }
    });

    // Configurar la generación de documentación XML para Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Agregar servicios de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Agregar servicios de aplicación
ApplicationInyector.Inyect(builder.Services);

// Agregar servicios de dominio
DomainInyector.Inyect(builder.Services);

// Agregar repositorios
builder.Services.AddRepositories();

var app = builder.Build();

// Configurar el middleware HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Habilitar Swagger solo en desarrollo
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Odoonto API v1"));
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Habilitar CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();