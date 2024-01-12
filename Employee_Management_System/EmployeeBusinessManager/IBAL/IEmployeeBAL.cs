using Employee_Management_System.Models;

namespace Employee_Management_System.EmployeeBusinessManager.IBAL
{
    public interface IEmployeeBAL
    {
        List<EmployeeModel> GetEmployeeList();

        public string AddEmployee(EmployeeModel employeeModel, IFormFile file);

        public EmployeeModel GetEmployeeById(int id);

        public string UpdateEmployee(int id, EmployeeModel employeeModel, IFormFile file);

        public string UploadImage(IFormFile imageFile);

        public bool CheckEmailExistence(string emailId);

        public bool CheckContactNoExistence(string contactNo);

        public void DeleteEmployee(int id);
    }
}
