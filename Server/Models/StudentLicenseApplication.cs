namespace Project.Models
{
    public class StudentLicenseApplication
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CourseTitle { get; set; }
        public DateTime Intake { get; set; }
        public string LicenceStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime? LicenceExpiryDate { get; set; }
    }
}
