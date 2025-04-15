using System.Threading.Tasks;

namespace Boilerplate.Domain.Auth.Interfaces;
public interface IHttpEmployeeService
{
    public  Task<EmployeeDateResponse?> GetEmployeeDataResponseAsync(EmployeeDateRequest request);
}
