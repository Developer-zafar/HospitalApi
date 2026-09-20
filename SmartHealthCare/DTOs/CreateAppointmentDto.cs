using System.ComponentModel.DataAnnotations;
namespace SmartHealthCare.DTOs
{
    public class CreateAppointmentDto
    {
        [Required]
        public int PatientId { get; set; }
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        public string Notes { get; set; }
    }
}
