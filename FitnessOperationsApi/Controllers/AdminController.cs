using FitnessOperationsApi.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessOperationsApi.Controllers;


[ApiController]
[Route("api/admin")]
[Authorize(Roles = Roles.Admin)]
public class AdminController : ControllerBase
{
    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {
        return Ok("Welcome Admin");
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpGet("all-users")]
    public IActionResult GetAllUsers()
    {
        return Ok();
    }
}


 