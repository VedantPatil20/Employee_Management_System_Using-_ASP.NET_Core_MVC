using Employee_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace Employee_Management_System.Controllers
{
    public class EmployeeController : Controller
    {
        public IConfiguration _configuration;
        public string connectionString;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Get All Employees
        public IActionResult Index()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    const string storedProcedureName = "GetAllEmployees";

                    using (MySqlCommand command = new MySqlCommand(storedProcedureName, connection))
                    {
                        // Set the command type to stored procedure
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        MySqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            EmployeeModel employeeModel = new EmployeeModel();

                            // Id, FirstName, LastName, ContactNo, Age

                            employeeModel.Id = (int)reader["Id"];
                            employeeModel.FirstName = reader["FirstName"].ToString();
                            employeeModel.LastName = reader["LastName"].ToString();
                            employeeModel.ContactNo = reader["ContactNo"].ToString();
                            employeeModel.EmailId = reader["EmailId"].ToString();
                            employeeModel.Age = (int)reader["Age"];
                            employeeModel.ImagePath = reader["ImagePath"].ToString();

                            employeeList.Add(employeeModel);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View(employeeList);

        }

        // Get All Employees
        public IActionResult EmployeesList()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    const string storedProcedureName = "GetAllEmployees";

                    using (MySqlCommand command = new MySqlCommand(storedProcedureName, connection))
                    {
                        // Set the command type to stored procedure
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        MySqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            EmployeeModel employeeModel = new EmployeeModel();

                            // Id, FirstName, LastName, ContactNo, Age

                            employeeModel.Id = (int)reader["Id"];
                            employeeModel.FirstName = reader["FirstName"].ToString();
                            employeeModel.LastName = reader["LastName"].ToString();
                            employeeModel.ContactNo = reader["ContactNo"].ToString();
                            employeeModel.EmailId = reader["EmailId"].ToString();
                            employeeModel.Age = (int)reader["Age"];
                            employeeModel.ImagePath = reader["ImagePath"].ToString();

                            employeeList.Add(employeeModel);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(employeeList);

        }

        private string UploadImage(IFormFile imageFile)
        {

            try
            {
                string uniqueFileName = null;

                if (imageFile != null)
                {
                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    // Create the directory if it doesn't exist

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }

                }
                else
                {
                    Console.WriteLine("Image file path is null");
                }

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Add new Employee Details
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeModel employee)
        {
            employee.ImagePath = UploadImage(employee.imageFile);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    const string queryString = "insert into employee (FirstName, LastName, ContactNo, EmailId, Age, ImagePath) values (@FirstName, @LastName, @ContactNo, @EmailId, @Age, @ImagePath);";

                    using (MySqlCommand command = new MySqlCommand(queryString, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@ContactNo", employee.ContactNo);
                        command.Parameters.AddWithValue("@EmailId", employee.EmailId);
                        command.Parameters.AddWithValue("@Age", employee.Age);
                        command.Parameters.AddWithValue("@ImagePath", employee.ImagePath);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
        
            return RedirectToAction("Index");
        }

        // Update existing employee details
        public IActionResult Edit(int? id)
        {

            EmployeeModel employeeModel = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    const string queryString = "SELECT * FROM employee WHERE Id = @Id";

                    using (MySqlCommand command = new MySqlCommand(queryString, connection))
                    {

                        command.Parameters.AddWithValue("@Id", id);

                        connection.Open();

                        MySqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            employeeModel = new EmployeeModel();

                            employeeModel.Id = (int)reader["Id"];
                            employeeModel.FirstName = reader["FirstName"].ToString();
                            employeeModel.LastName = reader["LastName"].ToString();
                            employeeModel.ContactNo = reader["ContactNo"].ToString();
                            employeeModel.EmailId = reader["EmailId"].ToString();
                            employeeModel.Age = (int)reader["Age"];
                            employeeModel.ImagePath = reader["imagePath"].ToString();

                        }
                    }
                }

            } 
            
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }

            return Json(employeeModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, EmployeeModel employee)
        {
            try
            {
                // get existing profile image from database
                string existingImage = null;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string queryString = "SELECT ImagePath FROM employee WHERE ID = @Id";

                    using (MySqlCommand command = new MySqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                existingImage = reader["ImagePath"].ToString();
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

                    employee.ImagePath = UploadImage(employee.imageFile);
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string queryString = "UPDATE employee SET FirstName = @FirstName, LastName = @LastName, ContactNo = @ContactNo, EmailId = @EmailId, Age = @Age, ImagePath = @ImagePath WHERE Id = @Id;";

                    using (MySqlCommand command = new MySqlCommand(queryString, connection))
                    {

                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@ContactNo", employee.ContactNo);
                        command.Parameters.AddWithValue("@EmailId", employee.EmailId);
                        command.Parameters.AddWithValue("@Age", employee.Age);
                        command.Parameters.AddWithValue("@ImagePath", employee.ImagePath);

                        connection.Open();

                        command.ExecuteNonQuery();

                    }
                }

            }

            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditData(int id, EmployeeModel employee)
        {
            try
            {
                // get existing profile image from database
                string existingImage = null;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string queryString = "SELECT ImagePath FROM employee WHERE ID = @Id";

                    using (MySqlCommand command = new MySqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                existingImage = reader["ImagePath"].ToString();
                            }
                        }
                    }
                }

                // If a new image file is uploaded, update the profile image

                if(employee.imageFile != null)
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

                    employee.ImagePath = UploadImage(employee.imageFile);
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string queryString = "UPDATE employee SET FirstName = @FirstName, LastName = @LastName, ContactNo = @ContactNo, EmailId = @EmailId, Age = @Age, ImagePath = @ImagePath WHERE Id = @Id;";

                    using (MySqlCommand command = new MySqlCommand(queryString, connection))
                    {

                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@ContactNo", employee.ContactNo);
                        command.Parameters.AddWithValue("@EmailId", employee.EmailId);
                        command.Parameters.AddWithValue("@Age", employee.Age);
                        command.Parameters.AddWithValue("@ImagePath", employee.ImagePath);

                        connection.Open();

                        command.ExecuteNonQuery();

                    }
                }

            }

            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }

            return RedirectToAction("Index");
        }

        // Delete employee from database
        public IActionResult Delete(int? id, EmployeeModel employee)
        {
            try
            {
                // get existing profile image from database
                string existingImage = null;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string queryString = "SELECT ImagePath FROM employee WHERE ID = @Id";

                    using (MySqlCommand command = new MySqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                existingImage = reader["ImagePath"].ToString();
                            }
                        }
                    }
                }

                // If a new image file is uploaded, update the profile image

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
                    string queryString = "DELETE FROM employee WHERE Id = @Id";

                    using (MySqlCommand command = new MySqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        connection.Open();

                        command.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Search()
        {
            return View();
        }

        // Search data by search keyword
        [HttpPost]
        public IActionResult Search(string Search)
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                const string queryString = "SELECT * FROM employee WHERE Id LIKE @Search OR FirstName LIKE @Search OR LastName LIKE @Search OR ContactNo = @Search OR EmailId LIKE @Search OR Age LIKE @Search;";

                using (MySqlCommand command = new MySqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@Search", "%" + Search + "%");

                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        EmployeeModel employeeModel = new EmployeeModel();

                        employeeModel.Id = (int)reader["Id"];
                        employeeModel.FirstName = reader["FirstName"].ToString();
                        employeeModel.LastName = reader["LastName"].ToString();
                        employeeModel.ContactNo = reader["ContactNo"].ToString();
                        employeeModel.EmailId = reader["EmailId"].ToString();
                        employeeModel.Age = (int)reader["Age"];

                        employeeList.Add(employeeModel);

                    }
                }
            }

            try
            {

            }

            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }

            return View("Index", employeeList);
        }



    }
}
