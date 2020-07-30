using System.Threading;
using System.Threading.Tasks;
using Coop.Sample.Core.Models;
using Coop.Sample.Core.Dtos;
using Coop.Sample.Core.Extensions;
using System.Collections.Generic;

namespace Coop.Sample.Core.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<EmployeeDto> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<EmployeeDetailsDto> GetByIdWithDetailsAsync(object id, CancellationToken cancellationToken);
        Task<EmployeeDto> GetOldestAsync(CancellationToken cancellationToken);
        Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken);
        Task<EmployeeDto> InsertAsync(EmployeePostDto employeePostDto, CancellationToken cancellationToken);
        Task<EmployeeDto> UpdateAsync(int id, EmployeePutDto employeePutDto, CancellationToken cancellationToken);
    }

    public class EmployeeRepository : IEmployeeRepository
    {
        public EmployeeRepository()
        {
        }

        public Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            // Mock
            List<EmployeeDto> data = new List<EmployeeDto> {
                new Employee() {
                    EmpNo = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Gender = "M"
                }.MapToDto()
            };

            return Task.Delay(1).ContinueWith(t => data);
        }

        public Task<EmployeeDto> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            EmployeeDto data = new Employee() {
                EmpNo = id,
                FirstName = "Dummy " + id,
                LastName = "name",
                Gender = "M"
            }.MapToDto();

            return Task.Delay(1).ContinueWith(t => data);
        }

        public Task<EmployeeDetailsDto> GetByIdWithDetailsAsync(object id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<EmployeeDto> GetOldestAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<EmployeeDto> InsertAsync(EmployeePostDto employeePostDto, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<EmployeeDto> UpdateAsync(int id, EmployeePutDto employeePutDto, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
