using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor to pass the DbContextOptions to the base class
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet to represent the StudentLicenseApplication table in the database
        public DbSet<StudentLicenseApplication> StudentLicenseApplications { get; set; }
    }
}
