namespace Employee_Management_System.Models
{
    public class EmployeeModel
    {
        public int id { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string emailId { get; set; }

        public string contactNo { get; set; }

        public string age { get; set; }

        public string profileImage { get; set; }

        public IFormFile imageFile { get; set; }

    }
}
