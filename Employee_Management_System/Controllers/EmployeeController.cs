using Employee_Management_System.EmployeeBusinessManager.BAL;
using Employee_Management_System.EmployeeBusinessManager.IBAL;
using Employee_Management_System.Models;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.Json;

namespace Employee_Mnagement_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;
        private string connectionString;
        IEmployeeBAL _IEmployeeBAL;

        public EmployeeController(IConfiguration configuration, IEmployeeBAL employeeBAL)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
            _IEmployeeBAL = employeeBAL;
        }

        public IActionResult Index()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            const string StoredProcedure = "GetEmployeeData";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(StoredProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        EmployeeModel employee = new EmployeeModel();

                        employee.id = (int)reader["emp_id"];
                        employee.firstName = reader["first_name"].ToString();
                        employee.lastName = reader["last_name"].ToString();
                        employee.emailId = reader["email_id"].ToString();
                        employee.contactNo = reader["contact_no"].ToString();
                        employee.age = reader["emp_age"].ToString();
                        employee.profileImage = reader["profile_image"].ToString();

                        employeeList.Add(employee);
                    }
                }
            }
            return View(employeeList);
        }

        // *List of all employees
        public IActionResult EmployeesList()
        {
            return Json(_IEmployeeBAL.GetEmployeeList());
        }

        // *View Add New Employee from
        public IActionResult Create()
        {
            return View();
        }

        // *Add New Employee
        [HttpPost, RequestSizeLimit(25 * 1000 * 1024)]
        public IActionResult Create(string model, IFormFile file)
        {
            EmployeeModel employee = JsonSerializer.Deserialize<EmployeeModel>(model)!;

            var result = _IEmployeeBAL.AddEmployee(employee, file);

            if (result == "EmailExists")
            {
                return Json("Email Id Already Exists!");
            }
            else if (result == "ContactNoExists")
            {
                return Json("Contact Number Already Exists!");
            }

            return Json("Index");

        }

        // *Populate form fields 
        public IActionResult GetEmployeeById(int id)
        {
            return Json(_IEmployeeBAL.GetEmployeeById(id));
        }

        // *Update Data
        [HttpPost, RequestSizeLimit(25 * 1000 * 1024)]
        public IActionResult Edit(int id, string model, IFormFile file)
        {

            EmployeeModel employee = JsonSerializer.Deserialize<EmployeeModel>(model)!;

            _IEmployeeBAL.UpdateEmployee(id, employee, file);
            
            return Json("Index");
        }

        // *Delete Existing Employee
        public IActionResult Delete(int id)
        {
            _IEmployeeBAL.DeleteEmployee(id);

            return RedirectToAction("Index");
        }

        // *Test Method
        public IActionResult Test()
        {
            return Json(_IEmployeeBAL.GetEmployeeList());
        }

    }
}