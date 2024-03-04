using FluentValidation;
using NTierArchitecture.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitecture.Business.Validator;
public sealed class LoginDtoValidator :AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(p => p.UserNameOrEmail).NotEmpty().WithMessage("Geçerli bir kullanıcı adı veya mail adresi giriniz!");
        RuleFor(p => p.UserNameOrEmail).MinimumLength(3).WithMessage("Geçerli bir kullanıcı adı veya mail adresi giriniz!");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Şifre boş olamaz!");
        RuleFor(p => p.Password).Matches("[A-Z]").WithMessage("Şifreniz en az bir büyük harf içermelidir!");
        RuleFor(p => p.Password).Matches("[a-z]").WithMessage("Şifreniz en az bir küçük harf içermelidir!");
        RuleFor(p => p.Password).Matches("[0-9]").WithMessage("Şifreniz en az bir rakam içermelidir!");
        RuleFor(p => p.Password).Matches("[^a-zA-Z0-9]").WithMessage("Şifreniz en az bir özel karakter içermelidir!");
    }
}
