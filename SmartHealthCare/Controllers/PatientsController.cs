using Microsoft.AspNetCore.Mvc;
using SmartHealthCare.DTOs;
using SmartHealthCare.Model;
using SmartHealthCare.Repositories;

namespace SmartHealthCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public PatientsController(IPatientRepository patientRepository, IDoctorRepository doctorRepository)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientRepository.GetAllAsync();
            var patientsDto = patients.Select(p => new PatientDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Gender = p.Gender,
                Contact = p.Contact,
                Condition = p.Condition,
                DoctorId = p.DoctorId,
                DoctorName = p.Doctor?.Name ?? "Unassigned"
            });

            return Ok(patientsDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                return NotFound($"Patient with Id {id} not found.");
            }

            var patientDto = new PatientDto
            {
                Id = patient.Id,
                Name = patient.Name,
                Age = patient.Age,
                Gender = patient.Gender,
                Contact = patient.Contact,
                Condition = patient.Condition,
                DoctorId = patient.DoctorId,
                DoctorName = patient.Doctor?.Name ?? "Unassigned"
            };

            return Ok(patientDto);
        }
        [HttpPost]
        public async Task<IActionResult> AddPatient([FromBody] CreatePatientDto patientDto)
        {
            // A. Check karo kya assigned doctor exist karta hai?
            var doctor = await _doctorRepository.GetByIdAsync(patientDto.DoctorId);
            if (doctor == null)
            {
                return BadRequest($"Doctor with Id {patientDto.DoctorId} does not exist.");
            }

            // B. DTO se Database Entity banayi:
            var patient = new Patients
            {
                Name = patientDto.Name,
                Age = patientDto.Age,
                Gender = patientDto.Gender,
                Contact = patientDto.Contact,
                Condition = patientDto.Condition,
                DoctorId = patientDto.DoctorId
            };

            // C. Database mein save karwaya:
            var createdPatient = await _patientRepository.AddAsync(patient);

            // D. Client ke liye clean Response DTO banaya:
            var responseDto = new PatientDto
            {
                Id = createdPatient.Id,
                Name = createdPatient.Name,
                Age = createdPatient.Age,
                Gender = createdPatient.Gender,
                Contact = createdPatient.Contact,
                Condition = createdPatient.Condition,
                DoctorId = createdPatient.DoctorId,
                DoctorName = doctor.Name
            };

            return CreatedAtAction(nameof(GetPatientById), new { id = responseDto.Id }, responseDto);
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePatient([FromBody] UpdatePatientDto patientDto)
        {
            var existingPatient = await _patientRepository.GetByIdAsync(patientDto.Id);
            if (existingPatient == null)
            {
                return NotFound($"Patient with Id {patientDto.Id} not found.");
            }
            // Doctor validation
            var doctor = await _doctorRepository.GetByIdAsync(patientDto.DoctorId);
            if (doctor == null)
            {
                return BadRequest($"Doctor with Id {patientDto.DoctorId} does not exist.");
            }
            // Update patient details
            existingPatient.Name = patientDto.Name;
            existingPatient.Age = patientDto.Age;
            existingPatient.Gender = patientDto.Gender;
            existingPatient.Contact = patientDto.Contact;
            existingPatient.Condition = patientDto.Condition;
            existingPatient.DoctorId = patientDto.DoctorId;
            await _patientRepository.UpdateAsync(existingPatient);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var existingPatient = await _patientRepository.GetByIdAsync(id);
            if (existingPatient == null)
            {
                return NotFound($"Patient with Id {id} not found.");
            }
            await _patientRepository.DeleteAsync(id);
            return NoContent();
        }

    }
}