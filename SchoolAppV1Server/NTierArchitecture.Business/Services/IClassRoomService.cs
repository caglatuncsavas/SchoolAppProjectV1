using NTierArchitecture.Entities.DTOs;
using NTierArchitecture.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitecture.Business.Services;
public interface IClassRoomService
{
    string Create(CreateClassRoomDto request);
    string Update(UpdateClassRoomDto request);
    string DeleteById(Guid Id);
    List<ClassRoom> GetAll();
}
