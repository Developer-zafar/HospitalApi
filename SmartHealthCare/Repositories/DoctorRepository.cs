    using Microsoft.EntityFrameworkCore;
    using SmartHealthCare.Data;
    using SmartHealthCare.Model;

    namespace SmartHealthCare.Repositories
    {
        public class DoctorRepository : IDoctorRepository
        {
            private readonly ApplicationDbContext _context;


            public DoctorRepository(ApplicationDbContext context)
            {
                _context = context;
            }


            public async Task<IEnumerable<Doctor>> GetAllAsync()
            {
                return await _context.Doctors.ToListAsync();
            }


            public async Task<Doctor?> GetByIdAsync(int id)
            {
                return await _context.Doctors.FindAsync(id);
            }
            public async Task DeleteAsync(int id)
            {
                var doctor = await _context.Doctors.FindAsync(id);
                if (doctor != null)
                {
                    _context.Doctors.Remove(doctor);
                    await _context.SaveChangesAsync();
                }
            }
            public async Task UpdateAsync(Doctor doctor)
            {
                _context.Entry(doctor).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            public async Task<Doctor> AddAsync(Doctor doctor)
            {
                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();
                return doctor;
        }

    }
    
    }