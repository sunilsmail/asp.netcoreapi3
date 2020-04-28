using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetCoreApi.Data;
using dotnetCoreApi.Filters;
using dotnetCoreApi.Installers;
using dotnetCoreApi.Options;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace dotnetCoreApi
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

            services.InstallServicesInAssembly(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // middleware code will go here 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
             app.UseDeveloperExceptionPage();
             app.UseCors(builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            //var swagger = new SwaggerGenOptions();
            //Configuration.GetSection(nameof(SwaggerGenOptions)).Bind(swagger);

            var swagger = Configuration.GetSection(nameof(SwaggerGenOptions)).Get<SwaggerGenOptions>();
            app.UseSwagger(option =>
            {
                option.RouteTemplate = swagger.JsonRoute;
            });
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swagger.UIEndpoint, swagger.Description);
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
