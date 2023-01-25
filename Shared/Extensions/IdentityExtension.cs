using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Business;
using Shared.Models.Dto;
using Shared.Services;




namespace Shared.Extensions
{
    public static class IdentityExtension
    {

        private static JwtBearerEvents jwtEvents = new JwtBearerEvents
        {
            //todo: JwtBearerEvents k�sm�nda kullan�c�.aktiftoken=token bilgisi kontrol� yap.
            //bir kullan�c� i�in ayn� anda bir token olu�turulmas�n�, kullanici.aktiftoken verisi ile tek token kullan�lmas�, signout ile SSO �zelli�i kat�labilir.

            //token do�rulanm��sa buras� tetiklenir
            OnTokenValidated = context =>
            {
                //ctx.SecurityToken.ToString()
                return Task.CompletedTask;
                //(new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler()).WriteToken(ctx.SecurityToken)
            },
            
            OnAuthenticationFailed = context =>
            {
                //context.Response.StatusCode = StatusCodes.Status200OK;
                //context.Response.ContentType = "application/json";
                //return Task.CompletedTask;
                //context.Response.WriteAsJsonAsync(Response<string>.Fail("Yetkisiz giri� denemesi", 401));
                return Task.CompletedTask;
            },

            //hatal� durumlarda OnAuthenticationFailed dan sonra tetiklenir.
            OnChallenge = context =>
            {
                //yetkisiz i�lem talebinde page status kodu 401 gitmesin, 200 gitsin. d�n�� model olarak detayl� d�ns�n.
                context.HandleResponse();
                var statusCode = context.Response.StatusCode;
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/plain";
               //return context.Response.WriteAsync("An error occurred processing your authentication.");
                return context.Response.WriteAsJsonAsync(Response<string>.Fail("Yetkisiz giri� denemesi", statusCode));
                //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //context.Response.ContentType = "text/plain";
                //ctx.Response.WriteAsync(message);
                //return Task.CompletedTask;
            },

            //T�m gelen validation bilgileri buray� tetitkler
            OnMessageReceived = context =>
            {

                //context.Request.Headers.TryGetValue("Authorization", out var BearerToken);
                //if (BearerToken.Count == 0)
                //    BearerToken = "no Bearer token sent\n";
                return Task.CompletedTask;
            }

        };


        public static void AddAuthentication(this IServiceCollection services, TokenOption tokenOption)
        {

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        SaveSigninToken = true,
                        ValidIssuer = tokenOption.Issuer,
                        ValidAudience = tokenOption.Audiences.FirstOrDefault(),
                        IssuerSigningKey = SignInService.GetSymmetricSecurityKey(tokenOption.SecurityKey),
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = jwtEvents;

                  

                });
        }



    }


}