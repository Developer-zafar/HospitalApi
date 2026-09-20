using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthCare.Data;
using SmartHealthCare.DTOs;
using SmartHealthCare.Model;
using SmartHealthCare.Services;

namespace SmartHealthCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase // 1. 'ControllerBase' ka 'B' capital fix ho gaya
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthController(
            ApplicationDbContext context,
            ITokenService tokenService,
            PasswordHasher<User> passwordHasher)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        // 1. REGISTER ENDPOINT
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            // Duplicate Email Check
            if (!await _context.User.AnyAsync(u => u.Email == registerDto.Email))
            {
                // DTO to Entity (with DoctorId & PatientId mapping!)
                var user = new User
                {
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    Role = registerDto.Role,
                    DoctorId = registerDto.DoctorId,
                    PatientId = registerDto.PatientId
                };

                // Password Hashing
                user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

                _context.User.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new { message = "User registered successfully." });
            }

            return BadRequest("Email is already in use.");
        }

        // 2. LOGIN ENDPOINT
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // Find User
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Verify Password Hash
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Generate JWT Token (carrying custom healthcare claims!)
            var token = _tokenService.GenerateToken(user);

            return Ok(new { token });
        }
    }
}