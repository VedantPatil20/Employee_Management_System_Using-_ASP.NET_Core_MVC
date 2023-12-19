using Employee_Management_System.Models;

namespace Employee_Management_System.EmployeeBusinessManager.IBAL
{
    public interface IEmployeeBAL
    {
        List<EmployeeModel> GetEmployeeList();

        public EmployeeModel AddEmployee(EmployeeModel employeeModel, IFormFile file);

        public string UploadImage(IFormFile imageFile);

    }
}
