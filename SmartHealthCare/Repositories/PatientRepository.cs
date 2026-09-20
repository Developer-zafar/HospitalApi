using Microsoft.EntityFrameworkCore;
using SmartHealthCare.Data;
using SmartHealthCare.Model;

namespace SmartHealthCare.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Doctor ka data sath load kiya:
        public async Task<IEnumerable<Patients>> GetAllAsync()
        {
            return await _context.Patients
                .Include(p => p.Doctor)
                .ToListAsync();
        }

        // 2. Doctor ke sath specific patient dhoonda:
        public async Task<Patients?> GetByIdAsync(int id)
        {
            return await _context.Patients
                .Include(p => p.Doctor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // 3. Add:
        public async Task<Patients> AddAsync(Patients patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        // 4. Update:
        public async Task UpdateAsync(Patients patient)
        {
            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // 5. Delete:
        public async Task DeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }

        // 6. Specific Doctor ke mareez (Doctor data ke sath):
        public async Task<IEnumerable<Patients>> GetPatientsByDoctorIdAsync(int doctorId)
        {
            return await _context.Patients
                .Where(p => p.DoctorId == doctorId)
                .Include(p => p.Doctor)
                .ToListAsync();
        }
    }
}