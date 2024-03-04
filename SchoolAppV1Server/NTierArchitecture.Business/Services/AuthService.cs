using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NTierArchitecture.Business.Constants;
using NTierArchitecture.Business.Validator;
using NTierArchitecture.Entities.DTOs;
using NTierArchitecture.Entities.Models;
namespace NTierArchitecture.Business.Services;

public sealed class AuthService(
    UserManager<AppUser> userManager,
    IJwtProvider jwtProvider) 
{
    public async Task<string> LoginAsync(LoginDto request)
    {
        AppUser? appUser =
            await userManager.Users
            .FirstOrDefaultAsync(p =>
            p.UserName == request.UserNameOrEmail ||
            p.Email == request.UserNameOrEmail);

        if(appUser is null)
        {
            throw new ArgumentException(MessageConstants.DataNotFound);
        }

        //Kullanıcı varsa şifreyi kontrol etmemiz lazım.
        bool result = await userManager.CheckPasswordAsync(appUser, request.Password);
        if (!result)
        {
            throw new ArgumentException(MessageConstants.PasswordIsWrong);
        }
            return jwtProvider.CreateToken();
    }
}
