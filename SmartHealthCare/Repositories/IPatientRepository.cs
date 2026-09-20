using SmartHealthCare.Model;

namespace SmartHealthCare.Repositories
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patients>> GetAllAsync();
        Task<Patients?> GetByIdAsync(int id);
        Task<Patients> AddAsync(Patients patient);
        Task UpdateAsync(Patients patient);
        Task DeleteAsync(int id);
        Task<IEnumerable<Patients>> GetPatientsByDoctorIdAsync(int doctorId);
    }
}