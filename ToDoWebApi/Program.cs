using AspNetCoreRateLimit;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Repository;
using Serilog;
using Service;
using Service.Contract;
using Shared.PropertyMapping;
using ToDoWebApi.ContentFormatters;

namespace ToDoWebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        // configure Serilog
        Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .WriteTo.File("logs/todoapi.txt", rollingInterval: RollingInterval.Day)
                        .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSerilog();

        builder.Services.SetupCors();// send requests from a different domain to our application
        builder.Services.Configure<IISOptions>(_ =>
        {
        });

        builder.Services.AddDbContext<ToDoContext>(options => options.UseSqlite("Data Source=todo.db", x => x.MigrationsAssembly("Repository")));

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();
        builder.Services.AddScoped<IApiAuthenticationService, ApiAuthenticationService>();
        builder.Services.AddTransient<IPropertyMappingService, PropertyMappingService>();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.SetupVersioning();
        builder.Services.AddResponseCaching();
        builder.Services.SetupHttpCacheHeaders();

        builder.Services.AddMemoryCache();
        // This library uses memory cache to store counters and rules. Therefore, we added
        // MemoryCache to service collection
        builder.Services.SetupRateLimitingOptions();
        builder.Services.AddHttpContextAccessor();

        builder.Services.SetupIdentity();
        builder.Services.SetupJWT(builder.Configuration);
        builder.Services.SetupSwagger();

        builder.Services.AddControllers(
            options =>
            {
                options.ReturnHttpNotAcceptable = true;
                options.OutputFormatters.Add(new CsvOutputFormatter());
                options.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
            })
        .AddNewtonsoftJson(setupAction => setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
        .AddXmlDataContractSerializerFormatters()
        .ConfigureApiBehaviorOptions(ValidationFailureBehavior());

        var app = builder.Build();
        app.UseMiddleware<GlobalExceptionMiddleware>();//should be first in pipeline

        if (app.Environment.IsDevelopment())
        {
            app.UsingSwagger();
        }
        else
        {
            app.UseHsts();
            // app.UsingSwagger();// nedavat na github
        }

        app.UseHttpsRedirection();//redirection from HTTP to HTTPS.

        app.UseIpRateLimiting();
        app.UseCors("CorsPolicy");
        app.UseResponseCaching();//Microsoft recommends having UseCors before UseResponseCaching
        app.UseHttpCacheHeaders();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.ResetDatabaseAsync();

        await app.RunAsync();
    }

    private static Action<ApiBehaviorOptions> ValidationFailureBehavior()
    {
        return setupAction =>
        {
            setupAction.InvalidModelStateResponseFactory = context =>
            {
                var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                var validationProblemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);

                // add additional info not added by default
                validationProblemDetails.Detail = "See the errors field for details.";
                validationProblemDetails.Instance = context.HttpContext.Request.Path;

                // report invalid model state responses as validation issues
                validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                validationProblemDetails.Title = "One or more validation errors occurred.";

                return new UnprocessableEntityObjectResult(validationProblemDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };
            };
        };
    }
}