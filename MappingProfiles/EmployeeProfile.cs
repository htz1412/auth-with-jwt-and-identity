using AuthImplementation.DataModels;
using AuthImplementation.Entities;
using AutoMapper;

namespace AuthImplementation.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDataModel>().ReverseMap();
        }
    }
}
