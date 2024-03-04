using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using NTierArchitecture.Business.Constants;
using NTierArchitecture.Business.Validator;
using NTierArchitecture.DataAccess.Repositories;
using NTierArchitecture.Entities.DTOs;
using NTierArchitecture.Entities.Models;

namespace NTierArchitecture.Business;

public sealed class StudentManager(
    IStudentRepository studentRepository,
    IMapper mapper): IStudentService
{
    public string Create(CreateStudentDto request)
    {
        CreateStudentDtoValidator validator = new();
        ValidationResult result=validator.Validate(request);
        if (!result.IsValid)
        {
            throw new ArgumentException(string.Join(",",",result.Errors"));
        }

        //TC Numarasının unıque olup olmadıgını kontrol et.Bunun için db'e bağlanıp bakmak gerekir.Db'ye direk bağlanmak istemıyorum contextı buaraya çağırmak mantıklı değil.
        bool isIdentityNumberExists= 
            studentRepository
            .Any(p=>p.IdentityNumber == request.IdentityNumber);

        if (isIdentityNumberExists)
        {
            throw new ArgumentException(MessageConstants.IdentityNumberAlreadyExist);
        }

        //bool isStudentNumberExists= studentRepository.Any(p=>p.StudentNumber == request.StudentNumber);

        //if(isStudentNumberExists)
        //{
        //    throw new Exception("Öğrenci numarası daha önce kaydedilmiş!");
        //}

        int studentNumber = studentRepository.GetNewStudentNumber();

        Student student = mapper.Map<Student>(request);
        student.StudentNumber = studentNumber;
        student.CreatedDate = DateTime.Now;
        student.CreatedBy = "Admin";

        studentRepository.Create(student);
        return MessageConstants.CreateIsSuccess;

    }

    public string DeleteById(Guid id)
    {
        studentRepository.DeleteById(id);
        return MessageConstants.DeleteIsSuccess;
    }

    public List<Student> GetAll()
    {
        List<Student> students = studentRepository
                                                .GetAll()
                                                .OrderBy(p=>p.ClassRoomId)
                                                .ThenBy(p=>p.FirstName)
                                                .ToList();
        return students;
    }

    public List<Student> GetAllByClassRoomId(Guid classRoomId)
    {
        List<Student> students = studentRepository
                                                .GetAll()
                                                .Where(p=>p.ClassRoomId == classRoomId)                                           
                                                .OrderBy(p => p.FirstName)
                                                .ToList();
        return students;
    }

    public string Update(UpdateStudentDto request)
    {
        UpdateStudentDtoValidator validator = new();
        ValidationResult result = validator.Validate(request);

        if (!result.IsValid)
        {
            throw new ValidationException(string.Join(",",result.Errors.Select(s=>s.ErrorMessage).ToList()));
        }

        Student? student = studentRepository.GetStudentById(request.Id);
        if(student is null)
        {
            throw new ArgumentException(MessageConstants.DataNotFound);
        }

        bool isIdentityNumberExists= 
            studentRepository
            .Any(p=>p.IdentityNumber == request.IdentityNumber && p.Id != request.Id);

        if (isIdentityNumberExists)
        {
            throw new ArgumentException(MessageConstants.IdentityNumberAlreadyExist);
        }

        mapper.Map(request,student);
        //student.IdentityNumber = request.IdentityNumber;
        //student.FirstName = request.FirstName;
        //student.LastName = request.Lastname;
        //student.ClassRoomId = request.ClassRoomId;
        student.UpdatedDate = DateTime.Now;
        student.UpdatedBy = "Admin";
        studentRepository.Update(student);

        return MessageConstants.UpdateIsSuccess;
    }
}
