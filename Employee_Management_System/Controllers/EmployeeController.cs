﻿using Employee_Management_System.EmployeeBusinessManager.BAL;
using Employee_Management_System.EmployeeBusinessManager.IBAL;
using Employee_Management_System.Models;
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

        // **List of all employees with JSON (AJAX Function)
        public IActionResult EmployeesList()
        {
            return Json(_IEmployeeBAL.GetEmployeeList());
        }

        // **
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, RequestSizeLimit(25 * 1000 * 1024)]
        public IActionResult Create(string model, IFormFile file)
        {
            EmployeeModel employee = JsonSerializer.Deserialize<EmployeeModel>(model)!;

            _IEmployeeBAL.AddEmployee(employee, file);

            return Json("Index");
        }

        public IActionResult populateEditData(int? id)
        {
            EmployeeModel employeeModel = null;

            const string StoredProcedure = "GetEmployeeById";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(StoredProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        employeeModel = new EmployeeModel();

                        // Id, FirstName, LastName, ContactNo, Age

                        employeeModel.id = (int)reader["emp_id"];
                        employeeModel.firstName = reader["first_name"].ToString();
                        employeeModel.lastName = reader["last_name"].ToString();
                        employeeModel.emailId = reader["email_id"].ToString();
                        employeeModel.contactNo = reader["contact_no"].ToString();
                        employeeModel.age = reader["emp_age"].ToString();
                        employeeModel.profileImage = reader["profile_image"].ToString();

                    }
                }
            }

            return Json(employeeModel);
        }

        [HttpPost, RequestSizeLimit(25 * 1000 * 1024)]
        public IActionResult Edit(int id, string model, IFormFile file)
        {

            EmployeeModel employee = JsonSerializer.Deserialize<EmployeeModel>(model)!;

            employee.id = id;

            employee.imageFile = file;

            // employee.ProfileImage = UploadImage(employee.imageFile);

            try
            {
                // Get existing profile image from the database
                string existingImage = null;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string StoredProcedure = "GetProfileImageById";

                    using (MySqlCommand command = new MySqlCommand(StoredProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", employee.id);

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                existingImage = reader["profile_image"].ToString();
                            }
                        }
                    }
                }

                // If a new image file is uploaded, update the profile image
                if (employee.imageFile != null)
                {
                    // Delete the old image file if it exists
                    if (!string.IsNullOrEmpty(existingImage))
                    {
                        string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", existingImage);

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    employee.profileImage = UploadImage(employee.imageFile);
                }
                else
                {
                    // If no new image is provided, use the existing image
                    employee.profileImage = existingImage;
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string StoredProcedure = "UpdateEmployee";

                    using (MySqlCommand command = new MySqlCommand(StoredProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", employee.id);
                        command.Parameters.AddWithValue("@FirstName", employee.firstName);
                        command.Parameters.AddWithValue("@LastName", employee.lastName);
                        command.Parameters.AddWithValue("@ContactNo", employee.contactNo);
                        command.Parameters.AddWithValue("@EmailId", employee.emailId);
                        command.Parameters.AddWithValue("@Age", employee.age);
                        command.Parameters.AddWithValue("@ProfileImage", employee.profileImage);

                        connection.Open();

                        command.ExecuteNonQuery();

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json("Index");
        }

        public IActionResult Delete(int? id)
        {

            // get existing profile image from database
            string existingImage = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // string queryString = "SELECT profile_image FROM employee WHERE emp_id = @Id;";

                string StoredProcedure = "GetProfileImageById";

                using (MySqlCommand command = new MySqlCommand(StoredProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            existingImage = reader["profile_image"].ToString();
                        }
                    }
                }
            }

            // Delete the old image file if it exists

            if (!string.IsNullOrEmpty(existingImage))
            {
                string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", existingImage);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // string queryString = "DELETE FROM employee WHERE emp_id = @Id";

                string StoredProcedure = "DeleteEmployee";

                using (MySqlCommand command = new MySqlCommand(StoredProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string Search)
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            // string Query = "SELECT * FROM employee WHERE emp_id = @search OR first_name LIKE @search OR last_name LIKE @search OR email_id LIKE @search OR contact_no = @search OR emp_age LIKE @search;";
            string StoredProcedure = "SearchEmployee";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(StoredProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@search", "%" + Search + "%");
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

                        employeeList.Add(employee);
                    }
                }
            }
            return View("Index", employeeList);
        }

        // Get Employee Details by ID
        public IActionResult GetEmployeeById(int? id)
        {
            EmployeeModel employeeModel = null;

            if (id == null)
            {
                // Handle null ID case, maybe return an error message or handle it as per your application's logic
                return BadRequest("Employee ID is null");
            }

            const string StoredProcedure = "GetEmployeeById"; // The stored procedure to fetch employee details

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(StoredProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employeeModel = new EmployeeModel();

                            // Populate the employeeModel object with retrieved data
                            employeeModel.id = (int)reader["emp_id"];
                            employeeModel.firstName = reader["first_name"].ToString();
                            employeeModel.lastName = reader["last_name"].ToString();
                            employeeModel.emailId = reader["email_id"].ToString();
                            employeeModel.contactNo = reader["contact_no"].ToString();
                            employeeModel.age = reader["emp_age"].ToString();
                            employeeModel.profileImage = reader["profile_image"].ToString();
                        }
                    }
                }
            }

            if (employeeModel == null)
            {
                // Handle the case where no employee with the given ID was found
                return NotFound("Employee not found");
            }

            return Json(employeeModel); // Return the employee details as JSON
        }

        // Upload Image to file system
        private string UploadImage(IFormFile imageFile)
        {
            try
            {
                string uniqueFileName = null;

                if (imageFile != null)
                {
                    // creating a string uploadFolder that contains the path to the images folder inside the wwwroot directory of your application
                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    // Create the directory if it doesn't exist
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    // Rename upload image name to some unique file name
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

                    // creating a string "filePath" that contains the full path to where the file will be saved.
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    // creating a new FileStream object that represents the file at filePath
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        // copying the contents of imageFile to the stream
                        imageFile.CopyTo(stream);
                    }
                }
                else
                {
                    Console.WriteLine("Image File is Null");
                }
                return uniqueFileName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public IActionResult Test()
        {
            return Json(_IEmployeeBAL.GetEmployeeList());
        }

    }
}