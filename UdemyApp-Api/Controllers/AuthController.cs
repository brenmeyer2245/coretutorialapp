using System.Text;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using UdemyApp.DB;
using UdemyApp.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;


namespace UdemyApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthRepository _repo;
        private IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config){
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO user){

            user.Username = user.Username.ToLower();

            if (await _repo.UserExists(user.Username)) {
                return BadRequest("User already exists");
            }

            var UserToCreate = new User {
                Username = user.Username
            };

            var createdUser = _repo.Register(UserToCreate, user.Password);
            return StatusCode(201);

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto) {


                //user is in DB?
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (null == userFromRepo) return Unauthorized();

            //Now I got my user, need to create the JWT

            //make my claims from the JWT I'm creating
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };


            //Creating my security key for validation

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));


            //create signing creds with key and the key encoded using HMACSHA512


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //descriptor will contain the expiring info and the credentials
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };


            //This allows us to create and write token using the token descriptor
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {token = tokenHandler.WriteToken(token)} );

    }
}
}
