using DocumentManagement.Api.Configurations;
using DocumentManagement.Api.Middlewares;
using DocumentManagement.Application;
using DocumentManagement.Application.Commons.Behaviors;
using FluentValidation;
using MediatR;
using Serilog;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build())
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();


            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerConfiguration();

            builder.AddDatabaseConfiguration();

            //Add Jwt Config
            builder.Services.AddJwtConfiguration(builder.Configuration);

            //Add DI Config
            builder.Services.AddDependencyInjectionConfiguration();

            //Add MediatR
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            builder.Services.AddRateLimiterConfiguration();

            // Add FluentValidation Validators
            builder.Services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);

            //Add Behaviors
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));


            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure Middleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<CorrelationIdMiddleware>();

            app.UseSerilogRequestLogging();

            app.UseRateLimiter();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Document Management System API v1");
                    options.DocumentTitle = "DMS API Documentation";
                });
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }

    }
}