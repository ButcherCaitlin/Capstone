using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Capstone.API.Configuration;
using Capstone.API.Services;
using AutoMapper;
using System;
using Microsoft.AspNetCore.Http;

namespace Capstone.API
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
            services.Configure<CapstoneDatabaseSettings>(
                Configuration.GetSection(nameof(CapstoneDatabaseSettings)));
            services.AddSingleton<ICapstoneDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<CapstoneDatabaseSettings>>().Value);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //you need to add a service here for each additional collection.
            services.AddSingleton<PropertyService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<ShowingService>();

            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                /*setupAction.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter());*/ // this is the old way to add an xml formatter
            }).AddXmlDataContractSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected server fault occured. Please try again later");
                        //this is where you would log the fault.
                    });
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
