using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Auth;
public class EmployeeDateResponse
{
    public List<EmployeeDateEmployee> Data { get; set; }= new List<EmployeeDateEmployee>();

}

public class EmployeeDateEmployee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool IsActive { get; set; }
    public double Salary { get; set; }
    public DateTime JoinDate { get; set; }
}


public class EmployeeDateRequest
{
    public string Param1 { get; set; } = string.Empty;
    public int Param2 { get; set; } 
    public bool Param3 { get; set; }
    public double Param4 { get; set; }
    public DateTime Param5 { get; set; }
    public Guid Param6 { get; set; }
}

