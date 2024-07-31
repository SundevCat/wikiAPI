using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using wikiAPI.Models;
using wikiAPI.Services;

namespace wikiAPI.Controllers;

[ApiController]
[Authorize(Roles = "admin")] //Token =  eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsImlzcyI6ImxvY2FsaG9zdCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0In0.GAg2gkCXYGeLZy6I0bMvu9_B2-vl7zX7y3hsxfHDyO4
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUserAll() => await _userService.Users();

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        var user = await _userService.UserById(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<User>> GetUserByUsername(string username)
    {
        var user = await _userService.UserByUserName(username);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        var checkExist = await _userService.UserByUserName(user.UserName);
        if (checkExist != null)
        {
            return Conflict(new { message = "Username alerdy exists." });
        }
        await _userService.CreateUser(user);
        return Ok(user);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser([FromBody] User user, string id)
    {
        var checkExist = await _userService.UserById(id);
        if (checkExist == null)
        {
            return NoContent();
        }
        await _userService.UpdateUser(id, user);
        return Ok(new { message = "User has been update." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var checkExist = await _userService.UserById(id);
        if (checkExist == null)
        {
            return NotFound();
        }
        await _userService.DeleteUser(id);
        return Ok(new { message = "User has been delete." });
    }

}
