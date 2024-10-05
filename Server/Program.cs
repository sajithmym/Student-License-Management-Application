using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Services;
using DotNetEnv;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load environment variables from .env file
            Env.Load();

            // Construct the connection string from environment variables
            var host = Environment.GetEnvironmentVariable("host");
            var db = Environment.GetEnvironmentVariable("db");
            var user = Environment.GetEnvironmentVariable("user");
            var password = Environment.GetEnvironmentVariable("password");
            var connectionString = $"Server={host};Database={db};User={user};Password={password};";

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            builder.Services.AddScoped<IStudentLicenseService, StudentLicenseService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}