using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Interfaces;
public interface IOperationScoped
{
    string OperationId { get; }
}
