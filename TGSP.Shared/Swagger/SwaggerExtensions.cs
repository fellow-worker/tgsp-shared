using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace TGSP.Shared.Swagger
{
    /// <summary>
    /// This static provider easy swagger setup
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// This method will add swagger to the application
        /// </summary>
        /// <param name="services">The service collection to add swagger to</param>
        /// <param name="options">The config options for swagger</param>
        public static void AddSwagger(this IServiceCollection services, SwaggerOptions options)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(options.Version, new OpenApiInfo  { Title = options.Title, Version = options.Version });

                c.IncludeXmlComments(options.XmlDocumentation);
                c.IgnoreObsoleteProperties();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = $"You can use here the authorization header that is send by the application.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey
                        },
                        new List<string>()
                    }
                });
            });

            // services.ConfigureSwaggerGen(options => { options.CustomSchemaIds(x => x.FullName); });
        }

        /// <summary>
        /// Use swagger
        /// </summary>
        /// <param name="app">the application to add swagger to</param>
        /// <param name="options">The config options for swagger</param>
        public static void UseSwagger(this IApplicationBuilder app, SwaggerOptions options)
        {
             // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"swagger/{options.Version}/swagger.json", options.Version);
                c.RoutePrefix = string.Empty;
            });
        }
 
        /// <summary>
        /// This is a default version for the swagger options based on the entry assembly
        /// </summary>
        public static SwaggerOptions GetDefaultSwaggerOptions()
        {
            var name = Assembly.GetEntryAssembly().GetName().Name;
            return new SwaggerOptions()
            {
                Title = name,
                Version = "v1",
                XmlDocumentation = Path.Combine(AppContext.BaseDirectory, $"{name}.xml")
            };
        }
    }
}