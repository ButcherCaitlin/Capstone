using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Capstone.API.Configuration;
using Capstone.API.Repositories;
using Capstone.API.Services;
using AutoMapper;
using System;
using Microsoft.AspNetCore.Http;
using Capstone.API.Converters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Capstone.API.Entities;

namespace Capstone.API
{
    public class Startup
    {
        // Docker command for rebuilding the docker image: 
        // docker build -t aspnetapp .
        // docker run -d -p 8080:80 --name myapp aspnetapp
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
            //services.AddSingleton<PropertyRepository>();
            //services.AddSingleton<ShowingRepository>();
            //services.AddSingleton<RepositoryBase<User>>();
            services.AddSingleton<DataService>();
            services.AddSingleton<AuthenticationService>();

            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            })
            .AddNewtonsoftJson( setupAction =>
            {
                setupAction.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlDataContractSerializerFormatters()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new TimeSpanToStringConverter());
            })
            .ConfigureApiBehaviorOptions(setupAction => 
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetailsFactory = context.HttpContext.RequestServices
                        .GetRequiredService<ProblemDetailsFactory>();
                    var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                        context.HttpContext, context.ModelState);

                    problemDetails.Detail = "See error field for details.";
                    problemDetails.Instance = context.HttpContext.Request.Path;

                    var actionExecutingContext = context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                    if((context.ModelState.ErrorCount > 0) &&
                    (actionExecutingContext?.ActionArguments.Count ==
                    context.ActionDescriptor.Parameters.Count))
                    {
                        problemDetails.Type = "Capstone/ModelValidationProblem";
                        problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                        problemDetails.Title = "One or more validation errors occurred.";

                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };

                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "One or more input errors occured.";
                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });
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
