using AuthImplementation.DataModels;

namespace AuthImplementation.Services.Interfaces
{
    public interface IEmployeeService
    {
        public Task<List<EmployeeDataModel>> GetAllEmployees();
    }
}
