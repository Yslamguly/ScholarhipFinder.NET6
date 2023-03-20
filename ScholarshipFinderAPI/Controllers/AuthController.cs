using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScholarhipFinderAPI.Configurations;
using ScholarhipFinderAPI.Models;
using ScholarhipFinderAPI.Models.DTOs;

namespace ScholarhipFinderAPI.Contrllers;
[Route("/api/[controller]")] //api/auth
[ApiController]

public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    // private readonly JwtConfig _config;
    private readonly IConfiguration _configuration;
    public AuthController(UserManager<IdentityUser> userManager,IConfiguration configuration)//, JwtConfig config)
    {
        _userManager = userManager;
        _configuration = configuration;
    }
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody]UserResgistrationRequestDto requestedDto)
    {
        //validate the incoming request
        if(ModelState.IsValid){
            //Check if the email already exists
            var user_exists = await _userManager.FindByEmailAsync(requestedDto.Email);

            if(user_exists != null){
                return BadRequest(new AuthResult{
                    Result = false,
                    Errors = new List<string>{"Email already exists"}
                });
            }
            //create a user
            var newUser = new IdentityUser(){
                UserName = requestedDto.UserName,
                Email = requestedDto.Email
            };

            var is_cretaed = await _userManager.CreateAsync(newUser,requestedDto.Password);

            if(is_cretaed.Succeeded){
                //Generate tokens
                var token = GenerateToken(newUser);
                return Ok(new AuthResult(){
                    Result = true,
                    Token = token 
                });
            }
            return BadRequest(new AuthResult{
                Result = false,
                Errors = new List<string>{"Server Error"}
            });
        }
        return BadRequest();
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody]LoginRegistrationDto requestedDto){
        if(ModelState.IsValid){
            //Check if the user exists
            var user_exists = await _userManager.FindByEmailAsync(requestedDto.Email);
            if(user_exists == null){
                return BadRequest(new AuthResult{
                    Result = false,
                    Errors = new List<string>{"Invalid Payload"}
                });
            }
            var isCorrect = await _userManager.CheckPasswordAsync(user_exists,requestedDto.Password);

            if(!isCorrect){
                return BadRequest(new AuthResult{
                    Result = false,
                    Errors = new List<string>{"Invalid Credentials"}
                });
            }
            var token = GenerateToken(user_exists);
                return Ok(new AuthResult(){
                    Result = true,
                    Token = token 
                });
        }
        return BadRequest(new AuthResult(){
            Errors = new List<string>{"Invalid Payload"},
            Result = false
        });
    }
    private string GenerateToken(IdentityUser identityUser){
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

        var tokenDescriptor = new SecurityTokenDescriptor(){
            Subject = new ClaimsIdentity(new []{
                new Claim("Id",identityUser.Id),
                new Claim(JwtRegisteredClaimNames.Sub,identityUser.Email),
                new Claim(JwtRegisteredClaimNames.Email,identityUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToUniversalTime().ToString()),
            }),
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token); 

        return jwtToken;
    }
}