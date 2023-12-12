namespace Employee_Management_System.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ContactNo { get; set; }

        public string EmailId { get; set; }

        public int Age { get; set; }

        public string ImagePath { get; set; }

        public IFormFile imageFile { get; set; }

    }
}
