using System.ComponentModel.DataAnnotations;
namespace SmartHealthCare.DTOs
{
    public class CreatePatientDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Contact { get; set; }
        [Required]
        public string Condition { get; set; }
        [Required]
        public int DoctorId { get; set; }
    }
}
