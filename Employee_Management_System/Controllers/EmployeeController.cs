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
        IEmployeeBAL _IEmployeeBAL;

        public EmployeeController(IConfiguration configuration, IEmployeeBAL employeeBAL)
        {
            _IEmployeeBAL = employeeBAL;
        }

        public IActionResult Index()
        {
            return View();
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
            else if (result == "EmailAndContactNoExists")
            {
                return Json("Email Id and Contact Number Already Exists!");
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

            var result = _IEmployeeBAL.UpdateEmployee(id, employee, file);

            if (result == "EmailExists")
            {
                return Json("Email Id Already Exists!");
            }
            else if (result == "ContactNoExists")
            {
                return Json("Contact Number Already Exists!");
            }
            else if (result == "EmailAndContactNoExists")
            {
                return Json("Email Id and Contact Number Already Exists!");
            }
            
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