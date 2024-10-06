namespace Project.Models
{
    public class StudentLicenseApplication
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Licencepicture_path { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Institute { get; set; } = string.Empty;
        public string CourseTitle { get; set; } = string.Empty;
        public DateTime Intake { get; set; }
        public bool? LicenceStatus { get; set; } = false;
        public bool? ApprovalStatus { get; set; } = false;
        public DateTime? LicenceExpiryDate { get; set; }
    }
}