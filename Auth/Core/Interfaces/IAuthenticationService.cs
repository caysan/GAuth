using Core.Models.Dto;
using Shared.Models.Dto;

namespace Core.Interfaces
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Kullanýcý adý ve þifre üzerinden token üretir.
        /// </summary>
        Task<Response<Token>> CreateTokenByUserAsync(SignInByUser signIn);
        Task<Response<Token>> CreateTokenByLdapAsync(SignInByLdap signIn);
        Task<Response<Token>> RefreshTokenAsync(string refreshToken);
        Task<Response<string>> RefreshTokenDeleteAsync(string refreshToken);
        //Response<ClientToken> CreateTokenByClient(ClientSignIn clientSignIn);
    }
}