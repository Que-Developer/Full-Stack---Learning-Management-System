using backend.Data;
using backend.Models;
using backend.Models.Entities;
using backend.Models.Entities.Authentication;
using backend.Models.Entities.Authentication.SignUp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public AuthenticationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        [HttpPost("Register")]              // Adding/Creating Lecturer and Admin
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            // Check if role is valid (Admin or Lecturer)
            if (registerUser.Role != "Admin" && registerUser.Role != "Lecturer")
            {
                return BadRequest(new Response { Status = "Error", Message = "Please enter a valid role"});
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
                    else if (registerUser.Role == "Admin")
                    {
                        Admin adminEntry = new Admin()
                        {
                            Name = registerUser.Name,
                            Surname = registerUser.Surname,
                            Email = registerUser.Email
                        };

                        _dbContext.Admins.Add(adminEntry);
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

        [HttpPost("Register-Student")]
        public async Task<IActionResult> RegisterStudent(RegisterStudentDto registerStudentDto)
        {
            var studentRegistration = new RegisterStudent()
            {
                Name = registerStudentDto.Name,
                Surname = registerStudentDto.Surname,
                Gender = registerStudentDto.Gender,
                DateOfBirth = registerStudentDto.DateOfBirth,
                HomeAddress = registerStudentDto.HomeAddress,
                EmailAddress = registerStudentDto.EmailAddress,
                PhoneNumber = registerStudentDto.PhoneNumber,
                IsProcessed = false,
                Result = "pending",
                Password = registerStudentDto.Password,
                ConfirmPassword = registerStudentDto.ConfirmPassword,
                CourseID = registerStudentDto.CourseID
            };

            if (studentRegistration.CourseID != null || studentRegistration.CourseID > 100)
            {
                return BadRequest(new Response { Status = "Error", Message = "Please try again" });
            }

            _dbContext.StudentRegistrations.Add(studentRegistration);
            _dbContext.SaveChanges();

            return Ok(new Response { Status = "Success!", Message = $"{studentRegistration.Name} registered as Student" });
        }

        // Login for our registered Students/Lecturers and Admin
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUser loginUser)
        {
            var user = await  _userManager.FindByEmailAsync(loginUser.Email);

            if (user == null)
            {
                return BadRequest(new Response { Status = "Error", Message = "User Email does not exist" });
            }
            
            if (!await _userManager.CheckPasswordAsync(user, loginUser.ConfirmPassword))
            {
                return BadRequest(new Response { Status = "Error", Message = "Incorrect Password" });
            }

            return Ok(new Response { Status = "Success!", Message = "Login Successful!" });
        }

    }
}
