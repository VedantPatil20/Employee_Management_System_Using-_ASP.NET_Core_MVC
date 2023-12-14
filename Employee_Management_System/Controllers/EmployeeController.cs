﻿using Employee_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace Employee_Mnagement_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            // const string Query = "select * from employee;";

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

                        employee.Id = (int)reader["emp_id"];
                        employee.FirstName = reader["first_name"].ToString();
                        employee.LastName = reader["last_name"].ToString();
                        employee.EmailId = reader["email_id"].ToString();
                        employee.ContactNo = reader["contact_no"].ToString();
                        employee.Age = (int)reader["emp_age"];
                        employee.ProfileImage = reader["profile_image"].ToString();

                        employeeList.Add(employee);
                    }
                }
            }
            return View(employeeList);
        }

        // List of all employees with JSON (AJAX Function)
        public IActionResult EmployeesList()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            // const string Query = "select * from employee;";

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

                        employee.Id = (int)reader["emp_id"];
                        employee.FirstName = reader["first_name"].ToString();
                        employee.LastName = reader["last_name"].ToString();
                        employee.EmailId = reader["email_id"].ToString();
                        employee.ContactNo = reader["contact_no"].ToString();
                        employee.Age = (int)reader["emp_age"];
                        employee.ProfileImage = reader["profile_image"].ToString();

                        employeeList.Add(employee);
                    }
                }
            }
            return Json(employeeList);
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
                            employeeModel.Id = (int)reader["emp_id"];
                            employeeModel.FirstName = reader["first_name"].ToString();
                            employeeModel.LastName = reader["last_name"].ToString();
                            employeeModel.EmailId = reader["email_id"].ToString();
                            employeeModel.ContactNo = reader["contact_no"].ToString();
                            employeeModel.Age = (int)reader["emp_age"];
                            employeeModel.ProfileImage = reader["profile_image"].ToString();
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


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeModel employee)
        {
            employee.ProfileImage = UploadImage(employee.imageFile);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // const string Query = "Insert into employee (first_name, last_name, email_id, contact_no, emp_age, profile_image) values (@FirstName, @LastName, @EmailId, @ContactNo, @Age, @ProfileImage);";

                const string StoredProcedure = "InsertEmployee";

                using (MySqlCommand command = new MySqlCommand(StoredProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@EmailId", employee.EmailId);
                    command.Parameters.AddWithValue("@ContactNo", employee.ContactNo);
                    command.Parameters.AddWithValue("@Age", employee.Age);
                    command.Parameters.AddWithValue("@ProfileImage", employee.ProfileImage);

                    command.ExecuteNonQuery();
                }
            }

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

                        employeeModel.Id = (int)reader["emp_id"];
                        employeeModel.FirstName = reader["first_name"].ToString();
                        employeeModel.LastName = reader["last_name"].ToString();
                        employeeModel.EmailId = reader["email_id"].ToString();
                        employeeModel.ContactNo = reader["contact_no"].ToString();
                        employeeModel.Age = (int)reader["emp_age"];
                        employeeModel.ProfileImage = reader["profile_image"].ToString();

                    }
                }
            }

            if (employeeModel != null)
            {
                IFormFile imageFile = HttpContext.Request.Form.Files["imageFile"];

                if (imageFile != null)
                {
                    string uniqueFileName = UploadImage(imageFile);
                    employeeModel.ProfileImage = uniqueFileName;
                }
            }

            return Json(employeeModel);
        }

        [HttpPost]
        public IActionResult Edit([FromBody] EmployeeModel employee)
        {
            employee.ProfileImage = UploadImage(employee.imageFile);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string StoredProcedure = "UpdateEmployee";

                    using (MySqlCommand command = new MySqlCommand(StoredProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", employee.Id);
                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@ContactNo", employee.ContactNo);
                        command.Parameters.AddWithValue("@EmailId", employee.EmailId);
                        command.Parameters.AddWithValue("@Age", employee.Age);
                        command.Parameters.AddWithValue("@ProfileImage", employee.ProfileImage);

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

                        employee.Id = (int)reader["emp_id"];
                        employee.FirstName = reader["first_name"].ToString();
                        employee.LastName = reader["last_name"].ToString();
                        employee.EmailId = reader["email_id"].ToString();
                        employee.ContactNo = reader["contact_no"].ToString();
                        employee.Age = (int)reader["emp_age"];

                        employeeList.Add(employee);
                    }
                }
            }
            return View("Index", employeeList);
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

    }
}