﻿using Employee_Management_System.CommonCode;
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
            _dBManager.InitDbCommand("InsertEmployee")

            .AddCMDParam("@FirstName", employeeModel.firstName)
            .AddCMDParam("@LastName", employeeModel.lastName)
            .AddCMDParam("@EmailId", employeeModel.emailId)
            .AddCMDParam("@ContactNo", employeeModel.contactNo)
            .AddCMDParam("@Age", employeeModel.age)
            .AddCMDParam("@ProfileImage", employeeModel.profileImage)

            .ExecuteNonQuery();

            return employeeModel;
        }

        public EmployeeModel GetEmployeeById(int id)
        {
            _dBManager.InitDbCommand("GetEmployeeById");

            EmployeeModel employeeModel = null;

            _dBManager.AddCMDParam("@Id", id);

            DataSet ds = _dBManager.ExecuteDataSet();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                employeeModel = new EmployeeModel();

                // employeeModel.Id = CommonConversion.ConvertDBNullToInt(item["emp_id"]);
                employeeModel.id = item["emp_id"].ConvertDBNullToInt();
                employeeModel.firstName = item["first_name"].ConvertDBNullToString();
                employeeModel.lastName = item["last_name"].ConvertDBNullToString();
                employeeModel.emailId = item["email_id"].ConvertJSONNullToString();
                employeeModel.contactNo = item["contact_no"].ConvertJSONNullToString();
                employeeModel.age = item["emp_age"].ConvertJSONNullToString();
                employeeModel.profileImage = item["profile_image"].ConvertJSONNullToString();

            }

            return employeeModel;
        }

        public EmployeeModel UpdateEmployee(EmployeeModel employeeModel)
        {
            _dBManager.InitDbCommand("UpdateEmployee");

            _dBManager.AddCMDParam("@Id", employeeModel.id);
            _dBManager.AddCMDParam("@FirstName", employeeModel.firstName);
            _dBManager.AddCMDParam("@LastName", employeeModel.lastName);
            _dBManager.AddCMDParam("@ContactNo", employeeModel.contactNo);
            _dBManager.AddCMDParam("@EmailId", employeeModel.emailId);
            _dBManager.AddCMDParam("@Age", employeeModel.age);
            _dBManager.AddCMDParam("@ProfileImage", employeeModel.profileImage);

            _dBManager.ExecuteNonQuery();

            return employeeModel;
        }

        public void DeleteEmployee(int id)
        {
            _dBManager.InitDbCommand("DeleteEmployee");

            _dBManager.AddCMDParam("@Id", id);

            _dBManager.ExecuteNonQuery();
        }

        public string GetProfileImageById(int id)
        {
            string existingImage = null;

            _dBManager.InitDbCommand("GetProfileImageById");

            _dBManager.AddCMDParam("@Id", id);

            DataSet ds = _dBManager.ExecuteDataSet();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                existingImage = item["profile_image"].ConvertJSONNullToString();
            }

            return existingImage;
        }

        public bool CheckEmailExistence(string emailId)
        {
            _dBManager.InitDbCommand("CheckEmailExist");

            _dBManager.AddCMDParam("@p_EmailId", emailId);

            var result = _dBManager.ExecuteScalar(); // object to return single value

            bool emailExists = Convert.ToBoolean(result);

            return emailExists;
        }

        public bool CheckContactNoExistence(string contactNo)
        {
            _dBManager.InitDbCommand("CheckContactNoExist");

            _dBManager.AddCMDParam("@p_ContactNo", contactNo);

            var result = _dBManager.ExecuteScalar(); // object to return single value

            bool contactNoExists = Convert.ToBoolean(result); 

            return contactNoExists;

        }
    }
}
