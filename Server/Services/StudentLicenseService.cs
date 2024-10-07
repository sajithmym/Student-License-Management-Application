using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Services
{
    /// <summary>
    /// Service class for managing student license applications.
    /// </summary>
    public class StudentLicenseService : IStudentLicenseService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentLicenseService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public StudentLicenseService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Checks if a student with the specified email exists.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the student exists.</returns>
        public async Task<bool> StudentExists(string email)
        {
            return await _context.StudentLicenseApplications.AnyAsync(s => s.Email == email);
        }

        /// <summary>
        /// Adds a new student license application.
        /// </summary>
        /// <param name="application">The application to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added application.</returns>
        /// <exception cref="System.Exception">Thrown when a student with the same email already exists.</exception>
        public async Task<StudentLicenseApplication> AddStudentLicenseAsync(StudentLicenseApplication application)
        {
            if (await StudentExists(application.Email))
            {
                throw new System.Exception("A student with the same email already exists.");
            }

            _context.StudentLicenseApplications.Add(application);
            await _context.SaveChangesAsync();
            return application;
        }

        /// <summary>
        /// Retrieves all student license applications.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of applications.</returns>
        public async Task<List<StudentLicenseApplication>> GetAllApplicationsAsync()
        {
            return await _context.StudentLicenseApplications.ToListAsync();
        }

        /// <summary>
        /// Retrieves a student license application by ID.
        /// </summary>
        /// <param name="id">The ID of the application to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the application, or null if not found.</returns>
        public async Task<StudentLicenseApplication?> GetStudentLicenseByIdAsync(int id)
        {
            return await _context.StudentLicenseApplications.FindAsync(id);
        }

        /// <summary>
        /// Updates an existing student license application.
        /// </summary>
        /// <param name="application">The application to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the update was successful.</returns>
        public async Task<bool> UpdateStudentLicenseAsync(StudentLicenseApplication application)
        {
            var existingApplication = await _context.StudentLicenseApplications.FindAsync(application.Id);
            if (existingApplication == null)
            {
                return false;
            }

            // Update the fields of the existing application
            existingApplication.CourseTitle = application.CourseTitle;
            existingApplication.LicenceStatus = application.LicenceStatus;
            existingApplication.ApprovalStatus = application.ApprovalStatus;
            existingApplication.LicenceExpiryDate = application.LicenceExpiryDate;
            // Update other fields as necessary

            _context.StudentLicenseApplications.Update(existingApplication);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes a student license application by ID.
        /// </summary>
        /// <param name="id">The ID of the application to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the deletion was successful.</returns>
        public async Task<bool> DeleteStudentLicenseAsync(int id)
        {
            var application = await _context.StudentLicenseApplications.FindAsync(id);
            if (application == null)
            {
                return false;
            }

            _context.StudentLicenseApplications.Remove(application);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}