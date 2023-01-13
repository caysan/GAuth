using Core.Interfaces;
using Core.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : BaseController
{
    private readonly IAuthenticationService _authService;

    public AuthController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTokenByUser(SignInByUser signIn)
    {
        var result = await _authService.CreateTokenByUserAsync(signIn);

        return ActionResultInstance(result);
    }

    //[HttpPost]
    //public IActionResult CreateTokenByClient(ClientSignIn signIn)
    //{
    //    var result = _authService.CreateTokenByClient(signIn);

    //    return ActionResultInstance(result);
    //}

    [HttpPost]
    public async Task<IActionResult> RefreshTokenDelete(string refreshToken)
    {
        var result = await _authService.RefreshTokenDeleteAsync(refreshToken);

        return ActionResultInstance(result);
    }


    /// <summary>
    /// Token deðerini yeniler.
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RefreshToken(string refreshToken)
    {
        //var strUrl = HttpUtility.UrlDecode("refreshToken"); //api ile gönderirken url encode edilmiyor.
        //var result = await _authService.RefreshTokenAsync(strUrl);
        var result = await _authService.RefreshTokenAsync(refreshToken);

        return ActionResultInstance(result);
    }
}
