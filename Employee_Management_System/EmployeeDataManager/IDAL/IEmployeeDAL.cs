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
        public EmployeeModel PopulateUpdateData(int id)
        {
            return null;
        }
    }
}
