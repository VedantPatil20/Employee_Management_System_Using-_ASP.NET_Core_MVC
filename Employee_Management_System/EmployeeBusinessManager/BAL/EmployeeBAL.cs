using Employee_Management_System.EmployeeBusinessManager.IBAL;
using Employee_Management_System.EmployeeDataManager.DAL;
using Employee_Management_System.EmployeeDataManager.IDAL;
using Employee_Management_System.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Employee_Management_System.EmployeeBusinessManager.BAL
{
    public class EmployeeBAL : IEmployeeBAL
    {
        IEmployeeDAL _IEmployeeDAL;

        public EmployeeBAL(IDBManager dBManager)
        {
            _IEmployeeDAL = new EmployeeDAL(dBManager);
        }

        // **List All Employees
        List<EmployeeModel> IEmployeeBAL.GetEmployeeList()
        {
            return _IEmployeeDAL.GetEmployeeList();
        }

        // **Add New Employee
        public string AddEmployee(EmployeeModel employeeModel, IFormFile file)
        {
            employeeModel.imageFile = file;

            employeeModel.profileImage = UploadImage(employeeModel.imageFile);

            bool emailExists = CheckEmailExistence(employeeModel.emailId);

            bool contactNoExists = CheckContactNoExistence(employeeModel.contactNo);

            if (emailExists)
            {
                return "EmailExists";
            }
            else if (contactNoExists)
            {
                return "ContactNoExists";
            }
            else
            {
                _IEmployeeDAL.AddEmployee(employeeModel);
                return "Success";
            }
        }

        public EmployeeModel GetEmployeeById(int id)
        {
            return _IEmployeeDAL.GetEmployeeById(id);
        }

        public EmployeeModel UpdateEmployee(int id, EmployeeModel employeeModel, IFormFile file)
        {
            employeeModel.id = id;

            employeeModel.imageFile = file;

            string existingImage = _IEmployeeDAL.GetProfileImageById(id);

            // If a new image file is uploaded, update the profile image
            if (employeeModel.imageFile != null)
            {
                if (!string.IsNullOrEmpty(existingImage))
                {
                    string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", existingImage);

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                employeeModel.profileImage = UploadImage(employeeModel.imageFile);
            }
            else
            {
                // If no new image is provided, use the existing image
                employeeModel.profileImage = existingImage;
            }

            return _IEmployeeDAL.UpdateEmployee(employeeModel);

        }

        public void DeleteEmployee(int id)
        {
            string existingImage = _IEmployeeDAL.GetProfileImageById(id);

            if (!string.IsNullOrEmpty(existingImage))
            {
                string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", existingImage);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _IEmployeeDAL.DeleteEmployee(id);
        }

        // **Upload Image to File System (~/wwwroot/images/...jpg)
        public string UploadImage(IFormFile imageFile)
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

        public bool CheckEmailExistence(string emailId)
        {
            return _IEmployeeDAL.CheckEmailExistence(emailId);
        }

        public bool CheckContactNoExistence(string contactNo)
        {
            return _IEmployeeDAL.CheckContactNoExistence(contactNo);
        }
    }
}
