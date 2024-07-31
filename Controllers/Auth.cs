
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using wikiAPI.Models;
using wikiAPI.Services;

namespace wikiAPI.Controllers;

[ApiController]
[Route("auth")]

public class AuthController : ControllerBase
{

    private readonly AuthService _authService;
    private readonly IConfiguration? _configuration;

    public AuthController(AuthService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }


    [HttpGet("auth")]
    public async Task<ActionResult<List<Auth>>> GetAllAuth() => await _authService.Auth();

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Auth auth)
    {
        if (string.IsNullOrEmpty(auth.authUsername) || string.IsNullOrEmpty(auth.authPassword) || string.IsNullOrEmpty(auth.role))
        {
            return BadRequest("Invalid client request");
        }
        // check authUsername and password
        var authModel = await _authService.CreateAuth(auth);
        if (authModel.authUsername == auth.authUsername && authModel.authPassword == auth.authPassword)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signicCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var clamis = new List<Claim>
            {
                new Claim(ClaimTypes.Name,auth.authUsername),
                new Claim(ClaimTypes.Role,auth.role),
            };

            var tokenOption = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: clamis,
                signingCredentials: signicCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOption);
            return Ok(new { Token = tokenString });
        }
        else
        {
            return Unauthorized();
        }
    }

}