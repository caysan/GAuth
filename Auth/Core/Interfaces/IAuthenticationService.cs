using Core.Models.Dto;
using Shared.Models.Dto;

namespace Core.Interfaces
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Kullan�c� ad� ve �ifre �zerinden token �retir.
        /// </summary>
        Task<Response<Token>> CreateTokenByUserAsync(SignInByUser signIn);
        Task<Response<Token>> CreateTokenByLdapAsync(SignInByLdap signIn);
        Task<Response<Token>> RefreshTokenAsync(string refreshToken);
        Task<Response<string>> RefreshTokenDeleteAsync(string refreshToken);
        //Response<ClientToken> CreateTokenByClient(ClientSignIn clientSignIn);
    }
}