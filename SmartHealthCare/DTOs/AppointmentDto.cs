using System.ComponentModel.DataAnnotations;
namespace SmartHealthCare.DTOs
{
    public class AppointmentDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]
        public String PatientName { get; set; }
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public String DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
    }
}
