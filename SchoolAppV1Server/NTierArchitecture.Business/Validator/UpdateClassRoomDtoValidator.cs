using FluentValidation;
using NTierArchitecture.Entities.DTOs;


namespace NTierArchitecture.Business.Validator;

public sealed class UpdateClassRoomDtoValidator : AbstractValidator<UpdateClassRoomDto>
{
    public UpdateClassRoomDtoValidator()
    {
        RuleFor(p=>p.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required").MinimumLength(3);
    }
}

