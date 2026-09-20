using Microsoft.AspNetCore.Mvc;
using SmartHealthCare.Repositories;
using SmartHealthCare.Model;
using SmartHealthCare.DTOs;

namespace SmartHealthCare.Controllers
{
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorsController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        // 1. GET ALL DOCTORS
        [HttpGet("api/doctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            var doctorDtos = doctors.Select(d => new DoctorDto
            {
                Id = d.Id,
                Name = d.Name,
                Specialization = d.Specialization,
                Email = d.Email,
                PhoneNumber = d.PhoneNumber
            });

            return Ok(doctorDtos);
        }

        // 2. GET DOCTOR BY ID
        [HttpGet("api/doctors/{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                return NotFound($"Doctor with Id {id} not found.");
            }

            var doctorDto = new DoctorDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specialization = doctor.Specialization,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber
            };

            return Ok(doctorDto);
        }

        // 3. CREATE DOCTOR
        [HttpPost("api/doctors")]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newDoctor = new Doctor
            {
                Name = doctorDto.Name,
                Specialization = doctorDto.Specialization,
                Email = doctorDto.Email,
                PhoneNumber = doctorDto.PhoneNumber
            };

            var createdDoctor = await _doctorRepository.AddAsync(newDoctor);

            var responseDto = new DoctorDto
            {
                Id = createdDoctor.Id,
                Name = createdDoctor.Name,
                Specialization = createdDoctor.Specialization,
                Email = createdDoctor.Email,
                PhoneNumber = createdDoctor.PhoneNumber
            };

            return CreatedAtAction(nameof(GetDoctorById), new { id = responseDto.Id }, responseDto);
        }

        // 4. UPDATE DOCTOR
        [HttpPut("api/doctors/{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] CreateDoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDoctor = await _doctorRepository.GetByIdAsync(id);
            if (existingDoctor == null)
            {
                return NotFound($"Doctor with Id {id} not found.");
            }

            existingDoctor.Name = doctorDto.Name;
            existingDoctor.Specialization = doctorDto.Specialization;
            existingDoctor.Email = doctorDto.Email;
            existingDoctor.PhoneNumber = doctorDto.PhoneNumber;

            await _doctorRepository.UpdateAsync(existingDoctor);

            return NoContent();
        }

        // 5. DELETE DOCTOR
        [HttpDelete("api/doctors/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var existingDoctor = await _doctorRepository.GetByIdAsync(id);
            if (existingDoctor == null)
            {
                return NotFound($"Doctor with Id {id} not found.");
            }

            await _doctorRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}