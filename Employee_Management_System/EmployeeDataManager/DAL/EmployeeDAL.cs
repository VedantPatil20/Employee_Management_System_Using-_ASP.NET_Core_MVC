using Employee_Management_System.CommonCode;
using Employee_Management_System.EmployeeDataManager.IDAL;
using Employee_Management_System.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace Employee_Management_System.EmployeeDataManager.DAL
{
    public class EmployeeDAL : IEmployeeDAL
    {
        readonly IDBManager _dBManager;

        public EmployeeDAL(IDBManager dBManager)
        {
            _dBManager = dBManager;
        }

        public List<EmployeeModel> GetEmployeeList()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            _dBManager.InitDbCommand("GetEmployeeData");

            DataSet ds = _dBManager.ExecuteDataSet();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                EmployeeModel employeeModel = new EmployeeModel();

                // employeeModel.Id = CommonConversion.ConvertDBNullToInt(item["emp_id"]);
                employeeModel.id = item["emp_id"].ConvertDBNullToInt();
                employeeModel.firstName = item["first_name"].ConvertDBNullToString();
                employeeModel.lastName = item["last_name"].ConvertDBNullToString();
                employeeModel.emailId = item["email_id"].ConvertJSONNullToString();
                employeeModel.contactNo = item["contact_no"].ConvertJSONNullToString();
                employeeModel.age = item["emp_age"].ConvertJSONNullToString();
                employeeModel.profileImage = item["profile_image"].ConvertJSONNullToString();

                employeeList.Add(employeeModel);

            }

            return employeeList;
        }

        // Add New Employee
        public EmployeeModel AddEmployee(EmployeeModel employeeModel)
        {
            _dBManager.InitDbCommand("InsertEmployee");

            _dBManager.AddCMDParam("@FirstName", employeeModel.firstName);
            _dBManager.AddCMDParam("@LastName", employeeModel.lastName);
            _dBManager.AddCMDParam("@EmailId", employeeModel.emailId);
            _dBManager.AddCMDParam("@ContactNo", employeeModel.contactNo);
            _dBManager.AddCMDParam("@Age", employeeModel.age);
            _dBManager.AddCMDParam("@ProfileImage", employeeModel.profileImage);

            _dBManager.ExecuteNonQuery();

            return employeeModel;
        }

        

    }
}
