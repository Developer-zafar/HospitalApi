using System.ComponentModel.DataAnnotations;
namespace SmartHealthCare.DTOs

{
    public class CreateDoctorDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Specialization { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
