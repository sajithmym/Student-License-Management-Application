using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Data;
using Project.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Server.Tests
{
    public class StudentLicenseServiceTests
    {
        private readonly StudentLicenseService _studentLicenseService;
        private readonly AppDbContext _dbContext;
        private readonly ITestOutputHelper _output;

        public StudentLicenseServiceTests(ITestOutputHelper output)
        {
            _output = output;

            // Setup in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentLicenseTestDb")
                .Options;

            _dbContext = new AppDbContext(options);

            // Initialize the service to test
            _studentLicenseService = new StudentLicenseService(_dbContext);
        }

        // This method runs before each test to ensure a clean state
        private void ClearDatabase()
        {
            _dbContext.StudentLicenseApplications.RemoveRange(_dbContext.StudentLicenseApplications);
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task AddStudentLicenseAsync_ShouldAddLicense_WhenDataIsValid()
        {
            // Ensure database is cleared before test
            ClearDatabase();

            // Arrange
            var application = new StudentLicenseApplication
            {
                Name = "Sajith",
                Email = "Sajithmym@gmail.com",
                Phone = "1234567890",
                Address = "123 Street",
                Country = "Srilanka",
                Institute = "University of kelaniya",
                CourseTitle = "Software Engineering",
                Intake = DateTime.Now,
            };

            // Act
            var result = await _studentLicenseService.AddStudentLicenseAsync(application);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Sajithmym@gmail.com", result.Email);
            Assert.True(await _dbContext.StudentLicenseApplications.AnyAsync(a => a.Email == "Sajithmym@gmail.com"));

            // Output to console
            _output.WriteLine("Test AddStudentLicenseAsync_ShouldAddLicense_WhenDataIsValid passed");
        }

        [Fact]
        public async Task AddStudentLicenseAsync_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Ensure database is cleared before test
            ClearDatabase();

            // Arrange: Add an initial application
            var application = new StudentLicenseApplication
            {
                Name = "Sajith",
                Email = "Sajithmym@gmail.com",
                Phone = "1234567890",
                Address = "123 Street",
                Country = "Srilanka",
                Institute = "University of kelaniya",
                CourseTitle = "Software Engineering",
                Intake = DateTime.Now,
            };
            await _dbContext.StudentLicenseApplications.AddAsync(application);
            await _dbContext.SaveChangesAsync();

            // Act & Assert: Trying to add another application with the same email should throw an exception
            var duplicateApplication = new StudentLicenseApplication
            {
                Name = "Jane Doe",
                Email = "Sajithmym@gmail.com", // Same email as before
                Phone = "0987654321",
                Address = "456 Street",
                Country = "Srilanka",
                Institute = "DEF University",
                CourseTitle = "Computer Science",
                Intake = DateTime.Now,
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _studentLicenseService.AddStudentLicenseAsync(duplicateApplication));

            // Output to console
            _output.WriteLine("Test AddStudentLicenseAsync_ShouldThrowException_WhenEmailAlreadyExists passed");
        }

        [Fact]
        public async Task GetAllApplicationsAsync_ShouldReturnAllApplications()
        {
            // Ensure database is cleared before test
            ClearDatabase();

            // Arrange: Add some applications
            var applications = new List<StudentLicenseApplication>
            {
                new StudentLicenseApplication
                {
                    Name = "Sajith",
                    Email = "Sajithmym@gmail.com",
                    Phone = "1234567890",
                    Address = "123 Street",
                    Country = "Srilanka",
                    Institute = "University of kelaniya",
                    CourseTitle = "Software Engineering",
                    Intake = DateTime.Now,
                },
                new StudentLicenseApplication
                {
                    Name = "Jane Doe",
                    Email = "jane.doe@example.com",
                    Phone = "0987654321",
                    Address = "456 Street",
                    Country = "Srilanka",
                    Institute = "DEF University",
                    CourseTitle = "Computer Science",
                    Intake = DateTime.Now,
                }
            };
            await _dbContext.StudentLicenseApplications.AddRangeAsync(applications);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _studentLicenseService.GetAllApplicationsAsync();

            // Assert
            Assert.Equal(2, result.Count);

            // Output to console
            _output.WriteLine("Test GetAllApplicationsAsync_ShouldReturnAllApplications passed");
        }

        [Fact]
        public async Task GetStudentLicenseByIdAsync_ShouldReturnApplication_WhenIdIsValid()
        {
            // Ensure database is cleared before test
            ClearDatabase();

            // Arrange: Add an application
            var application = new StudentLicenseApplication
            {
                Name = "Sajith",
                Email = "Sajithmym@gmail.com",
                Phone = "1234567890",
                Address = "123 Street",
                Country = "Srilanka",
                Institute = "University of kelaniya",
                CourseTitle = "Software Engineering",
                Intake = DateTime.Now,
            };
            await _dbContext.StudentLicenseApplications.AddAsync(application);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _studentLicenseService.GetStudentLicenseByIdAsync(application.Id!.Value);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(application.Email, result.Email);

            // Output to console
            _output.WriteLine("Test GetStudentLicenseByIdAsync_ShouldReturnApplication_WhenIdIsValid passed");
        }

        [Fact]
        public async Task GetStudentLicenseByIdAsync_ShouldReturnNull_WhenIdIsInvalid()
        {
            // Ensure database is cleared before test
            ClearDatabase();

            // Act
            var result = await _studentLicenseService.GetStudentLicenseByIdAsync(999);

            // Assert
            Assert.Null(result);

            // Output to console
            _output.WriteLine("Test GetStudentLicenseByIdAsync_ShouldReturnNull_WhenIdIsInvalid passed");
        }

        [Fact]
        public async Task UpdateStudentLicenseAsync_ShouldUpdateApplication_WhenDataIsValid()
        {
            // Ensure database is cleared before test
            ClearDatabase();

            // Arrange: Add an application
            var application = new StudentLicenseApplication
            {
                Name = "Sajith",
                Email = "Sajithmym@gmail.com",
                Phone = "1234567890",
                Address = "123 Street",
                Country = "Srilanka",
                Institute = "University of kelaniya",
                CourseTitle = "Software Engineering",
                Intake = DateTime.Now,
            };
            await _dbContext.StudentLicenseApplications.AddAsync(application);
            await _dbContext.SaveChangesAsync();

            // Modify the application
            application.Name = "John Updated";

            // Act
            var result = await _studentLicenseService.UpdateStudentLicenseAsync(application);

            // Assert
            Assert.True(result);
            var updatedApplication = await _dbContext.StudentLicenseApplications.FindAsync(application.Id);
            Assert.Equal("John Updated", updatedApplication!.Name);

            // Output to console
            _output.WriteLine("Test UpdateStudentLicenseAsync_ShouldUpdateApplication_WhenDataIsValid passed");
        }

        [Fact]
        public async Task UpdateStudentLicenseAsync_ShouldReturnFalse_WhenApplicationDoesNotExist()
        {
            // Ensure database is cleared before test
            ClearDatabase();

            // Arrange: Create a non-existent application
            var application = new StudentLicenseApplication
            {
                Id = 999,
                Name = "Non Existent",
                Email = "non.existent@example.com",
                Phone = "1234567890",
                Address = "123 Street",
                Country = "Srilanka",
                Institute = "University of kelaniya",
                CourseTitle = "Software Engineering",
                Intake = DateTime.Now,
            };

            // Act
            var result = await _studentLicenseService.UpdateStudentLicenseAsync(application);

            // Assert
            Assert.False(result);

            // Output to console
            _output.WriteLine("Test UpdateStudentLicenseAsync_ShouldReturnFalse_WhenApplicationDoesNotExist passed");
        }

        [Fact]
        public async Task DeleteStudentLicenseAsync_ShouldDeleteApplication_WhenIdIsValid()
        {
            // Ensure database is cleared before test
            ClearDatabase();

            // Arrange: Add an application
            var application = new StudentLicenseApplication
            {
                Name = "Sajith",
                Email = "Sajithmym@gmail.com",
                Phone = "1234567890",
                Address = "123 Street",
                Country = "Srilanka",
                Institute = "University of kelaniya",
                CourseTitle = "Software Engineering",
                Intake = DateTime.Now,
            };
            await _dbContext.StudentLicenseApplications.AddAsync(application);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _studentLicenseService.DeleteStudentLicenseAsync(application.Id!.Value);

            // Assert
            Assert.True(result);
            Assert.False(await _dbContext.StudentLicenseApplications.AnyAsync(a => a.Id == application.Id));

            // Output to console
            _output.WriteLine("Test DeleteStudentLicenseAsync_ShouldDeleteApplication_WhenIdIsValid passed");
        }

        [Fact]
        public async Task DeleteStudentLicenseAsync_ShouldReturnFalse_WhenIdIsInvalid()
        {
            // Ensure database is cleared before test
            ClearDatabase();

            // Act
            var result = await _studentLicenseService.DeleteStudentLicenseAsync(999);

            // Assert
            Assert.False(result);

            // Output to console
            _output.WriteLine("Test DeleteStudentLicenseAsync_ShouldReturnFalse_WhenIdIsInvalid passed");
        }
    }
}