namespace SmartHealthCare.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; }
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
    }


 }
