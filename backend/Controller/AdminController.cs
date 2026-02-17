using System.Reflection.Metadata.Ecma335;
using backend.Data;
using backend.Models.Entities;
using backend.Models.Entities.Authentication;
using backend.Models.Entities.Authentication.SignUp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        // Getting all Students that have registered to our System
        [HttpGet("Getting-Student-Registrations")]
        public IActionResult GetStudentRegistrations()
        {
            var allStudentRegistrations = _dbContext.StudentRegistrations.ToList();

            if (allStudentRegistrations == null)
            {
                return Ok(new Response { Status = "Pending Registrations", Message = "No Students have registered so far" });
            }

            return Ok(allStudentRegistrations);
        }

        // Getting all Lecturers that are registered in our System
        [HttpGet("Getting-All-Lecturers")]
        public async Task<IActionResult> GetAllLecturers()
        {
            var allLecturers = _dbContext.Lecturers.ToList();

            if (allLecturers == null)
            {
                return BadRequest(new Response { Status = "Error", Message = "No Lecturers registered yet" });
            }

            return Ok(allLecturers);
        }

        // Admin has a lot of tasks but first will be to assign Students to Course (Approving Registration)
        [HttpPost("Approve(Adding)-Student-Registration/{id}")]
        public async Task<IActionResult> ApproveStudent(int id)
        {
            // Find registration details
            var registration = await _dbContext.StudentRegistrations.FindAsync(id);

            if (registration == null || registration.IsProcessed)
            {
                return NotFound("Registration not found or already processed");
            }

            // Checking if the email already exists
            var userExists = await _userManager.FindByEmailAsync(registration.EmailAddress);

            if (userExists != null)
            {
                return BadRequest(new Response { Status = "Error", Message = "This email already exist, register with another one" });
            }

            // Check if passwords match
            if (registration.Password != registration.ConfirmPassword)
            {
                return BadRequest(new Response { Status = "Error", Message = "Passwords do not match!" });
            }

            IdentityUser user = new IdentityUser()
            {
                UserName = registration.Name + " " + registration.Surname,
                Email = registration.EmailAddress
            };

            var result = await _userManager.CreateAsync(user, registration.ConfirmPassword);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Student");
            }

            // Map the registration information to the Student Entity
            var newStudent = new Student
            {
                Name = registration.Name,
                Surname = registration.Surname,
                Gender = registration.Gender,
                DateOfBirth = registration.DateOfBirth,
                HomeAddress = registration.HomeAddress,
                EmailAddress = registration.EmailAddress,
                PhoneNumber = registration.PhoneNumber, 
                EnrollmentDate = DateTime.UtcNow
            };

            // Now we add these changes to the Student Table
            _dbContext.Students.Add(newStudent);

            // Update that the student has been approved
            registration.Result = "Approved!";
            registration.IsProcessed = true;

            // Save Changes
            await _dbContext.SaveChangesAsync();

            return Ok(new Response { Status = "Approved", Message = $"Student ID {newStudent.StudentID} Approved, Congratulations!" });
        }

        [HttpPost("Register-Lecturer")]              // Adding/Creating Lecturer and Admin
        public async Task<IActionResult> RegisterLecturer([FromBody] RegisterUser registerUser)
        {
            // Check if role is valid (Admin or Lecturer)
            if (registerUser.Role != "Lecturer")
            {
                return BadRequest(new Response { Status = "Error", Message = "Please enter a valid role" });
            }

            // Checking if the user already exists
            var userExists = await _userManager.FindByEmailAsync(registerUser.Email);

            if (userExists != null)
            {
                return BadRequest(new Response { Status = "Error", Message = "Can't register, this user already exists!" });
            }

            // Checking if the passwords match
            if (registerUser.Password != registerUser.ConfirmPassword)
            {
                return BadRequest("Passwords don't match, try again!");
            }

            // Creating the Identity User
            IdentityUser user = new IdentityUser()
            {
                UserName = registerUser.Name + " " + registerUser.Surname,
                Email = registerUser.Email
            };

            // We need to ensure the role exists in our database
            if (await _roleManager.RoleExistsAsync(registerUser.Role))
            {
                var result = await _userManager.CreateAsync(user, registerUser.ConfirmPassword);

                if (result.Succeeded)
                {
                    // Assign the role to the user
                    await _userManager.AddToRoleAsync(user, registerUser.Role);

                    if (registerUser.Role == "Lecturer")
                    {
                        Lecturer lecturerEntry = new Lecturer()
                        {
                            Name = registerUser.Name,
                            Surname = registerUser.Surname,
                            Email = registerUser.Email,
                            PhoneNumber = null,
                            AdminID = null,
                            CourseID = null,
                        };

                        _dbContext.Lecturers.Add(lecturerEntry);
                    }

                    // Important that we save the changes made to the database
                    await _dbContext.SaveChangesAsync();
                    // _dbContext.SaveChanges - this works as well
                }
            }
            else
            {
                return BadRequest(new Response { Status = "Error", Message = "The role entered does not exist in our database" });
            }

            return Ok(new Response { Status = "Success!", Message = $"{registerUser.Name} registered as {registerUser.Role}" });
        }

        // Updating Lecturer details
        [HttpPut("Lecturers/Update-Lecturer{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] RegisterUser updateUser)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return BadRequest(new Response { Status = "Error", Message = "User not found" });
            }

            user.UserName = updateUser.Name;
            user.Email = updateUser.Email;

            // Validating if the email already exists
            if (await _userManager.FindByEmailAsync(user.Email) != null)
            {
                return BadRequest(new Response { Status = "Error", Message = "Email entered already exists" });
            }

            return Ok(new Response { Status = "Success", Message = $"User ID {id}, updated" });
        }
    }
}
