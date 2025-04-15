using Boilerplate.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.Auth;
public interface IRequiredPermissionMetadata
{
    HashSet<PermissionRequirement> RequiredPermissions { get; }
}

public class PermissionRequirement
{
    public Permissions Permission { get; set; }
    public string BusinessDomain { get; set; } = string.Empty;
}