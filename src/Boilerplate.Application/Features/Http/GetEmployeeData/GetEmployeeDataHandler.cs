using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Boilerplate.Domain.Auth.Interfaces;
using Boilerplate.Domain.Auth;
using Mapster;

namespace Boilerplate.Application.Features.Identity.GetEmployeeData;

public class GetEmployeeDataHandler : IRequestHandler<GetEmployeeDataRequest, Result<GetEmployeeDateResponse>>
{
    private readonly IHttpEmployeeService _httpServiceTokenService;


    public GetEmployeeDataHandler(IHttpEmployeeService httpServiceTokenService)
    {
        _httpServiceTokenService = httpServiceTokenService;
    }

    public async Task<Result<GetEmployeeDateResponse>> Handle(GetEmployeeDataRequest request, CancellationToken cancellationToken)
    {

       
        var tokenResponse = await _httpServiceTokenService.GetEmployeeDataResponseAsync(request.Adapt<EmployeeDateRequest>());



        return tokenResponse.Adapt<GetEmployeeDateResponse>();
    }
}