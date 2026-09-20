using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHealthCare.Repositories;
using SmartHealthCare.Model;
using SmartHealthCare.DTOs;

namespace SmartHealthCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public AppointmentsController(
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        // 1. GET ALL APPOINTMENTS
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentRepository.GetAllAsync();

            var appointmentDtos = appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                PatientId = a.PatientId,
                PatientName = a.Patient != null ? a.Patient.Name : "Unknown",
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor != null ? a.Doctor.Name : "Unknown",
                AppointmentDate = a.AppointmentDate,
                Status = a.Status,
                Notes = a.Notes
            });

            return Ok(appointmentDtos);
        }

        // 2. GET APPOINTMENT BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound($"Appointment with Id {id} not found.");
            }

            var appointmentDto = new AppointmentDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient != null ? appointment.Patient.Name : "Unknown",
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor != null ? appointment.Doctor.Name : "Unknown",
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                Notes = appointment.Notes
            };

            return Ok(appointmentDto);
        }

        // 3. CREATE APPOINTMENT (WITH VALIDATIONS & DTO)
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto appointmentDto)
        {
            // A. Patient check
            var patient = await _patientRepository.GetByIdAsync(appointmentDto.PatientId);
            if (patient == null)
            {
                return BadRequest($"Patient with Id {appointmentDto.PatientId} does not exist.");
            }

            // B. Doctor check
            var doctor = await _doctorRepository.GetByIdAsync(appointmentDto.DoctorId);
            if (doctor == null)
            {
                return BadRequest($"Doctor with Id {appointmentDto.DoctorId} does not exist.");
            }

            // C. DTO to Entity
            var newAppointment = new Appointment
            {
                PatientId = appointmentDto.PatientId,
                DoctorId = appointmentDto.DoctorId,
                AppointmentDate = appointmentDto.AppointmentDate,
                Status = "Scheduled",
                Notes = appointmentDto.Notes
            };

            var createdAppointment = await _appointmentRepository.AddAsync(newAppointment);

            // D. Response DTO
            var responseDto = new AppointmentDto
            {
                Id = createdAppointment.Id,
                PatientId = createdAppointment.PatientId,
                PatientName = patient.Name,
                DoctorId = createdAppointment.DoctorId,
                DoctorName = doctor.Name,
                AppointmentDate = createdAppointment.AppointmentDate,
                Status = createdAppointment.Status,
                Notes = createdAppointment.Notes
            };

            return CreatedAtAction(nameof(GetAppointmentById), new { id = responseDto.Id }, responseDto);
        }

        // 4. UPDATE APPOINTMENT
        [HttpPut("{id}")]
        [Authorize(Policy = "DoctorOnly")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] CreateAppointmentDto appointmentDto)
        {
            var existingAppointment = await _appointmentRepository.GetByIdAsync(id);
            if (existingAppointment == null)
            {
                return NotFound($"Appointment with Id {id} not found.");
            }

            // Patient validation
            var patient = await _patientRepository.GetByIdAsync(appointmentDto.PatientId);
            if (patient == null)
            {
                return BadRequest($"Patient with Id {appointmentDto.PatientId} does not exist.");
            }

            // Doctor validation
            var doctor = await _doctorRepository.GetByIdAsync(appointmentDto.DoctorId);
            if (doctor == null)
            {
                return BadRequest($"Doctor with Id {appointmentDto.DoctorId} does not exist.");
            }

            // Update properties
            existingAppointment.PatientId = appointmentDto.PatientId;
            existingAppointment.DoctorId = appointmentDto.DoctorId;
            existingAppointment.AppointmentDate = appointmentDto.AppointmentDate;
            existingAppointment.Notes = appointmentDto.Notes;

            await _appointmentRepository.UpdateAsync(existingAppointment);

            return NoContent();
        }

        // 5. DELETE APPOINTMENT
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var existingAppointment = await _appointmentRepository.GetByIdAsync(id);
            if (existingAppointment == null)
            {
                return NotFound($"Appointment with Id {id} not found.");
            }

            await _appointmentRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}