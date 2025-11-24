using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using UserRegistration.Models;

namespace UserRegistration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public RegistrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]

        [Route("registration")]
        public IActionResult Register([FromBody] Registration user)
        {
            string query = "insert into registration(name,email,password,isactive)values(@name,@email,@password,@isactive)";
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            using SqlConnection con= new SqlConnection(sqlDataSource);
            {
                SqlCommand cmd= new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Isactive", user.Isactive);

                con.Open();
                int rows=cmd.ExecuteNonQuery();
                con.Close();
                if (rows > 0)
                    return Ok("user Register Sucessfully");
                else
                    return BadRequest("Error In Register");
            }


        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] Registration user)
        {
            string query = "select * from registration where email=@email and password=@password and isactive=1";
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);
              //  cmd.Parameters.AddWithValue("@Isactive", user.Isactive);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    con.Close();

                    return Ok("User Login");
                }
                else
                {
                    con.Close();
                    return Unauthorized("inavalid user");
                }
            }
        }
        [HttpPut]
        [Route("changepassword")]
        public IActionResult ChangePassword(string email,string oldPassword,string newPassword)
        {
            string query = "update registration set password=@newPassword where email=@email and password=@oldPassword";
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                SqlCommand cmd= new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@newPassword", newPassword);
                cmd.Parameters.AddWithValue("@oldPassword", oldPassword);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();
                if (rows > 0)
                
                    return Ok("Password Change Sucessfull");
                
                else
                
                    return BadRequest("invalid password or email");
                
            }
        }

    }

}
