using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Interfaces;
public interface IGlobalSettingService
{
    Task<T?> GetValueAsync<T>(string key);
}
