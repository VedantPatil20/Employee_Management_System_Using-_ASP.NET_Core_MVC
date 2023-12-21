using Employee_Management_System.Models;

namespace Employee_Management_System.EmployeeDataManager.IDAL
{
    public interface IEmployeeDAL
    {
        // Gell ALL Employees
        public List<EmployeeModel> GetEmployeeList();

        // Add Data to Database
        public EmployeeModel AddEmployee(EmployeeModel employeeModel);

        // Populate data to Form Fields 
        public EmployeeModel GetEmployeeById(int id);

        // Update data 
        public EmployeeModel UpdateEmployee(EmployeeModel employeeModel);

        public void DeleteEmployee(int id);

        public bool CheckEmailExistence(string emailId);

        public bool CheckContactNoExistence(string contactNo);

        // Get Profile Image By ID
        public string GetProfileImageById(int id);
    }
}
