using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project.Data;
using Project.Services;
using DotNetEnv;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a web application builder
            var builder = WebApplication.CreateBuilder(args);

            // Load environment variables from .env file
            Env.Load();

            // Construct the connection string using environment variables
            string connectionString = BuildConnectionString();

            // Register services with the dependency injection container
            ConfigureServices(builder.Services, connectionString);

            // Build the application
            var app = builder.Build();

            // Configure middleware
            Configure(app);

            // Run the application
            app.Run();
        }

        private static string BuildConnectionString()
        {
            var host = Environment.GetEnvironmentVariable("host");
            var db = Environment.GetEnvironmentVariable("db");
            var user = Environment.GetEnvironmentVariable("user");
            var password = Environment.GetEnvironmentVariable("password");
            return $"Server={host};Database={db};User={user};Password={password};";
        }

        private static void ConfigureServices(IServiceCollection services, string connectionString)
        {
            // Register DbContext with MySQL configuration
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Register application services
            services.AddScoped<IStudentLicenseService, StudentLicenseService>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            services.AddControllers();
            // Add MVC services
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }


        private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            // Add CORS middleware before the authorization middleware
            app.UseCors("AllowAllOrigins"); // <-- Add this line here

            app.UseAuthorization();

            app.MapControllers();
        }


    }
}