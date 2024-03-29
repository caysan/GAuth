using Core.Interfaces;
using Core.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost(nameof(CreateUser))]
        public async Task<IActionResult> CreateUser(CreateUser createUser)
        {
            return ActionResultInstance(await _userService.CreateUserAsync(createUser));
        }

        [HttpGet(nameof(GetUserByAktifUser))]
        [Authorize]
        public async Task<IActionResult> GetUserByAktifUser()
        {
            return ActionResultInstance(await _userService.GetUserByNameAsync(User?.Identity?.Name!));
        }
    }
}