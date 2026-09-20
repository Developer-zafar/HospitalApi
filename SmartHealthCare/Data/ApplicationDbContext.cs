using Microsoft.EntityFrameworkCore;
using SmartHealthCare.Model;

namespace SmartHealthCare.Data
{
   
    public class ApplicationDbContext : DbContext
    {
        internal object Users;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Patients> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> User { get; set; }
    }
}