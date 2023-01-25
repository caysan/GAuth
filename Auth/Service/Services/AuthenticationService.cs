using Core.Interfaces;
using Core.Models.Business;
using Core.Models.Dto;
using Core.Models.Entities;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Models.Dto;

namespace Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly List<Client> _clients;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshTokens> _userRefreshTokenRepository;

        public AuthenticationService(UserManager<AppUser> userManager, ITokenService tokenService, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshTokens> userRefreshTokenRepository, IOptions<List<Client>> clients)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _clients = clients.Value;
            _userRefreshTokenRepository = userRefreshTokenRepository;
        }

        public async Task<Response<Token>> CreateTokenByUserAsync(SignInByUser signIn)
        {
            if (signIn == null || string.IsNullOrWhiteSpace(signIn.UserName) || string.IsNullOrWhiteSpace(signIn.Password))
            {
                return Response<Token>.Fail("Kullan�c� ad� ve �ifresi giriniz");
            }

            //Kullan�c� ad� ve �ifre ile, DB User bilgileri �zerinden token olu�turulur.
            AppUser user = await _userManager.FindByNameAsync(signIn.UserName);
            if (user == null)
            {
                return Response<Token>.Fail("Ge�ersiz kullan�c� ad� ve �ifre.", 404);
            }

            if (!await _userManager.CheckPasswordAsync(user, signIn.Password))
            {
                return Response<Token>.Fail("Ge�ersiz kullan�c� ad� ve �ifre.", 404);
            }

            return await TokenOlustur(user);

            //var token = _tokenService.CreateToken(user);
            //var userRefreshToken = await _userRefreshTokenRepository.Where(w => w.UserId == user.Id).SingleOrDefaultAsync();
            //if (userRefreshToken == null)
            //{
            //    await _userRefreshTokenRepository.AddAsync(new UserRefreshToken
            //    {
            //        UserId = user.Id,
            //        Token = token.RefreshToken,
            //        Expiration = token.RefreshTokenExpiration
            //    });
            //}
            //else
            //{
            //    userRefreshToken.Token = token.RefreshToken;
            //    userRefreshToken.Expiration = token.RefreshTokenExpiration;
            //}
            //await _unitOfWork.CommitAsync();
            //return Response<Token>.Success(token, 200);
        }

        public async Task<Response<Token>> CreateTokenByLdapAsync(SignInByLdap signIn)
        {
            if (signIn == null || string.IsNullOrWhiteSpace(signIn.UserName) || string.IsNullOrWhiteSpace(signIn.Password))
            {
                return Response<Token>.Fail("Kullan�c� ad� ve �ifresi giriniz");
            }


            #region LDAP Do�rulama
            //TODO: Bu b�l�mde LDAP ile kullan�c� kontrol� yap�lacak.
            LdapUser ldapUser = new LdapUser(){ LdapUserName="cuneyt1" };
            //TODO: E�er login ba�ar�l� ise, _userManager.FindByNameAsync ile kullan�c� bak. varsa devam et, yoksa yeni _userManager.CreateAsync olu�tur.
            #endregion

            #region ldap do�rulama sonras� Kullan�c�n�n user tablosunun olu�turulmas�.
            var user = await _userManager.FindByNameAsync(ldapUser.LdapUserName);
            if (user == null)
            {
                var newAppUser = new AppUser() { 
                    UserName = ldapUser.LdapUserName,
                    Email = ""
                };
                var resultCreateUser = await _userManager.CreateAsync(newAppUser);
                if (!resultCreateUser.Succeeded)
                {
                    return Response<Token>.Fail(new Error(resultCreateUser.Errors.Select(s => s.Description).ToList()), 404);
                }
            }
            #endregion


            return await TokenOlustur(user);
        }




        private async Task<Response<Token>> TokenOlustur(AppUser user)
        {
            if (user==null || string.IsNullOrWhiteSpace(user.UserName))
            {
                return Response<Token>.Fail("Kullan�c� ad� girilmeli");
            }

            var token = _tokenService.CreateToken(user);
            var userRefreshToken = await _userRefreshTokenRepository.Where(w => w.UserId == user.Id).SingleOrDefaultAsync();
            if (userRefreshToken == null)
            {
                await _userRefreshTokenRepository.AddAsync(new UserRefreshTokens
                {
                    UserId = user.Id,
                    Token = token.RefreshToken,
                    Expiration = token.RefreshTokenExpiration
                });
            }
            else
            {
                userRefreshToken.Token = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();

            return Response<Token>.Success(token, 200);
        }



        //
        //public Response<ClientToken> CreateTokenByClient(ClientSignIn clientSignIn)
        //{
        //    var client = _clients.SingleOrDefault(s => s.Id == clientSignIn.ClientId && s.Secret == clientSignIn.ClientSecret);

        //    if (client == null)
        //    {
        //        return Response<ClientToken>.Fail("Client credentials are invalid.", 404, true);
        //    }

        //    var token = _tokenService.CreateToken(client);
        //    return Response<ClientToken>.Success(token, 200);
        //}


        public async Task<Response<Token>> RefreshTokenAsync(string refreshToken)
        {
            var entity = await _userRefreshTokenRepository.Where(w => w.Token == refreshToken && w.Expiration>DateTime.Now ).SingleOrDefaultAsync();
            if (entity == null)
            {
                return Response<Token>.Fail("Refresh token not found.", 404);
            }

            var user = await _userManager.FindByIdAsync(entity.UserId);
            if (user == null)
            {
                return Response<Token>.Fail("User not found.", 404);
            }

            var token = _tokenService.CreateToken(user);

            entity.Token = token.RefreshToken;
            entity.Expiration = token.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return Response<Token>.Success(token, 200);
        }

        public async Task<Response<string>> RefreshTokenDeleteAsync(string refreshToken)
        {
            var entity = await _userRefreshTokenRepository.Where(w => w.Token == refreshToken).SingleOrDefaultAsync();
            if (entity == null)
            {
                return Response<string>.Fail("Refresh token not found.", 404);
            }

            _userRefreshTokenRepository.Remove(entity);

            await _unitOfWork.CommitAsync();

            return Response<string>.Success(200);
        }
    }
}