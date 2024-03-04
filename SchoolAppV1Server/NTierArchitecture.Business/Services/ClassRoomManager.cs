using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using NTierArchitecture.Business.Constants;
using NTierArchitecture.Business.Validator;
using NTierArchitecture.DataAccess.Repositories;
using NTierArchitecture.Entities.DTOs;
using NTierArchitecture.Entities.Models;


namespace NTierArchitecture.Business.Services;

public sealed class ClassRoomManager(
    IClassRoomRepository classRoomRepository,
    IMapper mapper) : IClassRoomService
{
    public string Create(CreateClassRoomDto request)
    {
        CreateClassRoomDtoValidator validator = new();
        ValidationResult result =  validator.Validate(request);
        if (!result.IsValid)
        {
            throw new ValidationException(string.Join(",", result.Errors.Select(s => s.ErrorMessage).ToList()));
        }

        bool isClassRoomNameExists = classRoomRepository.Any(p => p.Name == request.Name);
        if(isClassRoomNameExists)
        {
            throw new ArgumentException(MessageConstants.NameAlreadyExist);
        }

        ClassRoom classRoom = mapper.Map<ClassRoom>(request);
        classRoom.CreatedBy = "Admin";
        classRoom.CreatedDate = DateTime.Now;

        classRoomRepository.Create(classRoom);

        return MessageConstants.CreateIsSuccess;
    }

    public string DeleteById(Guid Id)
    {
       classRoomRepository.DeleteById(Id);
        return MessageConstants.DeleteIsSuccess;
    }

    public List<ClassRoom> GetAll()
    {
        List<ClassRoom> classRooms = 
            classRoomRepository
            .GetAll()
            .OrderBy(p=>p.Name)
            .ToList();

        return classRooms;
    }

    public string Update(UpdateClassRoomDto request)
    {
        UpdateClassRoomDtoValidator validator = new();
        ValidationResult result= validator.Validate(request);

        if (!result.IsValid)
        {
            throw new ValidationException(string.Join(",", result.Errors.Select(s => s.ErrorMessage).ToList()));
        }

        ClassRoom? classRoom = classRoomRepository.GetClassRoomById(request.Id);
        if(classRoom is null)
        {
            throw new ArgumentException(MessageConstants.DataNotFound);
        }

        if(request.Name != classRoom.Name)
        {
            bool isClassRoomNameExists = classRoomRepository.Any(p => p.Name == request.Name );
            if(isClassRoomNameExists)
            {
                throw new ArgumentException(MessageConstants.NameAlreadyExist);
            }
        }

        mapper.Map(request, classRoom);
        classRoom.UpdatedBy = "Admin";
        classRoom.UpdatedDate = DateTime.Now;

        classRoomRepository.Update(classRoom);

        return MessageConstants.UpdateIsSuccess;
    }
}
