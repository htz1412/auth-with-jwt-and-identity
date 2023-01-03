using AuthImplementation.DataModels;
using AuthImplementation.Entities;
using AuthImplementation.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthImplementation.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;

        public EmployeeService(MyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDataModel>> GetAllEmployees()
        {
            var allEmployees = await _dbContext.Employees.ToListAsync();
            return _mapper.Map<List<EmployeeDataModel>>(allEmployees);
        }
    }
}
