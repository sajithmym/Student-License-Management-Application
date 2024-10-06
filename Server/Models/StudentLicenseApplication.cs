namespace Project.Models
{
    public class StudentLicenseApplication
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Licencepicture_path { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CourseTitle { get; set; } = string.Empty;
        public DateTime Intake { get; set; }
        public string LicenceStatus { get; set; } = string.Empty;
        public string ApprovalStatus { get; set; } = string.Empty;
        public DateTime LicenceExpiryDate { get; set; }
    }
}