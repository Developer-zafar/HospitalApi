namespace SmartHealthCare.Model
{
    public class Patients
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Condition { get; set; }
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public string Contact { get; internal set; }
    }
}
