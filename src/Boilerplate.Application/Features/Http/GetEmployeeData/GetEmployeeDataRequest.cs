using Ardalis.Result;
using Boilerplate.Domain.Auth;
using MediatR;
using System;
using System.Collections.Generic;

namespace Boilerplate.Application.Features.Identity.GetEmployeeData;



public class GetEmployeeDataRequest : IRequest<Result<GetEmployeeDateResponse>>
{
    public string Param1 { get; set; } = string.Empty;
    public int Param2 { get; set; }
    public bool Param3 { get; set; }
    public double Param4 { get; set; }
    public DateTime Param5 { get; set; }
    public Guid Param6 { get; set; }
}



public class GetEmployeeDateResponse
{
    public List<EmployeeDateEmployee> Data { get; set; }    =   new List<EmployeeDateEmployee>();

}