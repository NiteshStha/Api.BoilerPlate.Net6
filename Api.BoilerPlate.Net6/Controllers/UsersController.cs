using Api.BoilerPlate.Net6.Authorization;
using Api.BoilerPlate.Net6.Models.Users;
using Api.BoilerPlate.Net6.Services;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ems.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // POST api/users/signin
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> SignIn(AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model);
            return Ok(response);
        }

        // POST api/users/signup
        [Authorize(Role.Admin)]
        [HttpPost("[action]")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest model)
        {
            var user = await _userService.SignUp(model);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // GET: api/users
        [Authorize(Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        // GET api/users/1
        [Authorize(Role.Admin, Role.User)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // only admins can access other user records
            var currentUser = (User)HttpContext.Items["User"];
            if (id != currentUser.Id && currentUser.Role != Role.Admin)
                return Unauthorized(new { error = "Unauthorized" });

            var users = await _userService.GetById(id);
            return Ok(users);
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
