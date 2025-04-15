using Boilerplate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Infrastructure.Logging;
public class OperationScoped : IOperationScoped
{
    public string OperationId { get; } = Guid.NewGuid().ToString();
}
