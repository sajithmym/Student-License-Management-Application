using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System;

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
            try
            {
                return await _context.StudentLicenseApplications.AnyAsync(s => s.Email == email);
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not shown here)
                throw new Exception("An error occurred while checking if the student exists.", ex);
            }
        }

        /// <summary>
        /// Adds a new student license application.
        /// </summary>
        /// <param name="application">The application to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added application.</returns>
        /// <exception cref="System.Exception">Thrown when a student with the same email already exists.</exception>
        public async Task<StudentLicenseApplication> AddStudentLicenseAsync(StudentLicenseApplication application)
        {
            try
            {
                if (await StudentExists(application.Email))
                {
                    throw new Exception("A student with the same email already exists.");
                }

                _context.StudentLicenseApplications.Add(application);
                await _context.SaveChangesAsync();
                return application;
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not shown here)
                throw new Exception("An error occurred while adding the student license application.", ex);
            }
        }

        /// <summary>
        /// Retrieves all student license applications.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of applications.</returns>
        public async Task<List<StudentLicenseApplication>> GetAllApplicationsAsync()
        {
            try
            {
                return await _context.StudentLicenseApplications.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not shown here)
                throw new Exception("An error occurred while retrieving all applications.", ex);
            }
        }

        /// <summary>
        /// Retrieves a student license application by ID.
        /// </summary>
        /// <param name="id">The ID of the application to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the application, or null if not found.</returns>
        public async Task<StudentLicenseApplication?> GetStudentLicenseByIdAsync(int id)
        {
            try
            {
                return await _context.StudentLicenseApplications.FindAsync(id);
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not shown here)
                throw new Exception("An error occurred while retrieving the student license application by ID.", ex);
            }
        }

        /// <summary>
        /// Updates an existing student license application.
        /// </summary>
        /// <param name="application">The application to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the update was successful.</returns>
        public async Task<bool> UpdateStudentLicenseAsync(StudentLicenseApplication application)
        {
            try
            {
                var existingApplication = await _context.StudentLicenseApplications.FindAsync(application.Id);
                if (existingApplication == null)
                {
                    return false;
                }

                // Detach the existing tracked entity
                _context.Entry(existingApplication).State = EntityState.Detached;

                // Update the fields of the existing application
                _context.Entry(application).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not shown here)
                throw new Exception("An error occurred while updating the student license application.", ex);
            }
        }

        /// <summary>
        /// Deletes a student license application by ID.
        /// </summary>
        /// <param name="id">The ID of the application to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the deletion was successful.</returns>
        public async Task<bool> DeleteStudentLicenseAsync(int id)
        {
            try
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
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not shown here)
                throw new Exception("An error occurred while deleting the student license application.", ex);
            }
        }
    }
}