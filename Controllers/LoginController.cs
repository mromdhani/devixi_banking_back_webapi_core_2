
using Microsoft.AspNetCore.Authorization;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.Extensions.Configuration;  
using Microsoft.IdentityModel.Tokens;
using MonProjetBanking_Back.Models;
using System;  
using System.IdentityModel.Tokens.Jwt;  
using System.Security.Claims;  
using System.Text;  
  
namespace MonProjetBanking_Back.Controllers  
{  
    [Route("auth")]  
    [ApiController]  
    public class LoginController : ControllerBase  
    {  
        private IConfiguration _config;  
  
        public LoginController(IConfiguration config)  
        {  
            _config = config;  
        }  
        [AllowAnonymous]  
        [HttpPost]  
        public IActionResult Login(UserModel login)  
        {  
            IActionResult response = Unauthorized();  
            var user = AuthenticateUser(login);  
  
            if (user != null)  
            {  
                var tokenString = GenerateJSONWebToken(user);  
                response = Ok(new { token = tokenString });  
            }  
  
            return response;  
        }  
  
        private string GenerateJSONWebToken(UserModel userInfo)  
        {  
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));  
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);  
  
      var claims = new[] {  
        new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),  
        new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),         
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        
        new Claim("DateOfJoing", userInfo.DateOfJoing.ToString("yyyy-MM-dd")), 
        new Claim("Pouvoir", userInfo.Pouvoir),

    };  

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],  
              _config["Jwt:Issuer"],  
              claims,  
              expires: DateTime.Now.AddMinutes(120),  
              signingCredentials: credentials);  
  
            return new JwtSecurityTokenHandler().WriteToken(token);  
        }  
  
        private UserModel AuthenticateUser(UserModel login)  
        {  
            UserModel user = null;  
  
            //Validate the User Credentials  
            //Demo Purpose, I have Passed HardCoded User Information  
            if ((login.Username == "haifa")  && (login.Password == "password"))
            {  
                user = new UserModel { Username = "Haifa",
                                        EmailAddress = "haifa@gmail.com",
                                        Pouvoir = "admin",
                                        DateOfJoing = DateTime.Now 
                                     };  
            }  
              if((login.Username == "ameni")  && (login.Password == "password"))
            {  
                user = new UserModel {  Username = "Ameni", EmailAddress = "ameni@gmail.com",

                                        Pouvoir = "user",
                                        DateOfJoing = DateTime.Now  
                                      }; 
            }  
            return user;  
        }  
    }  
}