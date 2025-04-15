using Amazon.Runtime;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Boilerplate.Domain.Auth;
using Boilerplate.Domain.Auth.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Boilerplate.Infrastructure.Http;
public class HttpEmployeeService : IHttpEmployeeService
{
   
    private readonly DataHttpClientService _httpServiceClientService;

    public HttpEmployeeService( DataHttpClientService  httpServiceClientService)
    {
       
        _httpServiceClientService= httpServiceClientService;
    }

    public Task<EmployeeDateResponse?> GetEmployeeDataResponseAsync(EmployeeDateRequest request)
    {
      /*Process request*/

        return _httpServiceClientService.GetEmployeeData(request);

    }
}