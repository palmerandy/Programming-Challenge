using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Repository;
using Products.Services;
using Products.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace Products
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AddMvc(services);
            AddServices(services);
            AddSwagger(services);
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "My API", Version = "v1"});
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition(ApiKeyActionFilter.AuthorizationHeaderName, new ApiKeyScheme
                {
                    In = "header",
                    Description = $"Please enter API Key. Sample key: {ApiKeyActionFilter.AuthorizationApiKeyValue}",
                    Name = ApiKeyActionFilter.AuthorizationHeaderName,
                    Type = "apiKey",
                    Extensions = { }
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {ApiKeyActionFilter.AuthorizationHeaderName, Enumerable.Empty<string>()},
                });
            });
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IProductFactory, ProductFactory>();
        }

        private static void AddMvc(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ApiKeyActionFilter));
                options.Filters.Add(typeof(ModelStateActionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
