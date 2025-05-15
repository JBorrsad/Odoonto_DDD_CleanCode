namespace WF.Mimetic.UI.Server;

using IdentityModel.AspNetCore.OAuth2Introspection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WF.Mimetic.Data.Contexts;
using WF.Mimetic.Infraestructure.ExceptionsHandler;
using WF.Mimetic.Infraestructure.InversionOfControl;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {

        services.AddControllersWithViews().AddNewtonsoftJson();
        services.AddRazorPages();
        services.AddApplicationServices();

        if (Environment.IsDevelopment())
        {
            services.AddDataBaseDevelopContext();
        }
        else
        {
            services.AddDataBaseProductionContext(Configuration.GetConnectionString("DefaultConnection"));
        }

        services.AddSwaggerGen(conf =>
        {
            conf.UseAllOfForInheritance();

            conf.SelectSubTypesUsing(baseType =>
            {
                return baseType.Assembly.GetTypes().Where(type => type.IsSubclassOf(baseType));
            });

            conf.SelectDiscriminatorNameUsing((baseType) => "discriminator");
            conf.SelectDiscriminatorValueUsing((subType) => subType.Name);

            #region Bearer token
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            conf.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            conf.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            #endregion
        });

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        string[] corsOrigins = Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

        if (corsOrigins?.Length > 0)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder.WithOrigins(corsOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            });
        }

        #region Ateka
        IConfigurationSection authenticationSettings = Configuration.GetSection("AuthenticationSettings");

        services.AddAuthentication(OAuth2IntrospectionDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.SaveToken = true;
                 options.RequireHttpsMetadata = false;
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidIssuer = authenticationSettings.GetValue<string>("Authority"),
                     ValidAudience = authenticationSettings.GetValue<string>("ClientId"),
                     ClockSkew = TimeSpan.FromMinutes(5),
                     RequireSignedTokens = false,
                     ValidateIssuerSigningKey = false,
                     SignatureValidator = (token, parameters) =>
                     {
                         // Cambiamos a JsonWebToken en lugar de JwtSecurityToken
                         return new JsonWebToken(token);
                     }
                 };

                 options.Events = new JwtBearerEvents
                 {
                     OnTokenValidated = context =>
                     {
                         Console.WriteLine("Token validado correctamente");
                         return Task.CompletedTask;
                     },
                     OnAuthenticationFailed = context =>
                     {
                         Console.WriteLine($"Error de autenticaci�n: {context.Exception.Message}");
                         return Task.CompletedTask;
                     },
                     OnMessageReceived = context =>
                     {
                         var token = context.Token;
                         Console.WriteLine($"Token recibido: {token}");
                         return Task.CompletedTask;
                     }
                 };


                 // Gestion de eventos en hitos de la validaci�n del token
                 //options.Events = new JwtBearerEvents()
                 //{
                 //    OnAuthenticationFailed = (c) =>
                 //    {
                 //        Console.WriteLine($"OnAuthenticationFailed {c}");
                 //        return Task.CompletedTask;
                 //    },
                 //    OnTokenValidated = (c) =>
                 //    {
                 //        Console.WriteLine($"OnTokenValidated {c}");
                 //        return Task.CompletedTask;
                 //    },
                 //    OnForbidden = (c) =>
                 //    {
                 //        Console.WriteLine($"OnForbidden {c}");
                 //        return Task.CompletedTask;
                 //    },
                 //};
             });

        // Ponemos los scopes requeridos
        services.AddAuthorization(options =>
        {
            // Por defecto ponemos que todas las paginas tienen que estar autorizadas
            // y tienen que tener un scope o varios
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("scope", authenticationSettings.GetValue<string>("AccessScope"))
            .RequireRole(authenticationSettings.GetValue<string>("AuthorizedRoles").Split(","))
            .Build();

            // Podemos crear politicas para que nuestros endpoints tengan que tener ciertos scopes
            // [Authorize(Policy = "Read")]
            options.AddPolicy("Read", policy => policy.RequireClaim("scope", authenticationSettings.GetValue<string>("AccessScope")));
        });
        #endregion

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        // Es importante el order, si se pone primero UseAuthorization no va a funcionar porque no esta autentificado
        // el token
        // 1 - UseAuthentication
        // 2 - UseAuthorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors();

        if (Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseSwagger();
        string swaggerEndpoint = Configuration.GetSection("Swagger:Endpoint").Get<string>();
        string swaggerName = Configuration.GetSection("Swagger:Name").Get<string>();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint(swaggerEndpoint, swaggerName);
        });

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseHttpStatusCodeExceptionMiddleware();

        app.UseUnitOfWorkContextMiddleware();

        //app.UseSidecarPingMiddleware();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers().RequireAuthorization();
            endpoints.MapRazorPages();

            if (Environment.IsDevelopment())
            {
                endpoints.MapFallbackToFile("index.debug.html");
            }
            else
            {
                endpoints.MapFallbackToFile("index.html");
            }

        });
    }
}
