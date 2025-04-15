using Boilerplate.Application.Common;
using Boilerplate.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Boilerplate.Infrastructure.Setting;


public class SystemSettingRepository : IGlobalSettingService
{
    private readonly IContext _context;

    public SystemSettingRepository(IContext context)
    {
        _context = context;
    }

    public async Task<T?> GetValueAsync<T>(string key)
    {
        var setting = await _context.GlobalSetting.FirstOrDefaultAsync(s => s.Key == key);
        if (setting == null)
        {
            // eturn default value for type T or null
            return default;
        }

        if (typeof(T) == typeof(string))
        {
            return (T)Convert.ChangeType(setting.Value, typeof(T));
        }
        else if (typeof(T) == typeof(int))
        {
            if (int.TryParse(setting.Value, out var intValue))
            {
                return (T)Convert.ChangeType(intValue, typeof(T));
            }
        }
        else if (typeof(T) == typeof(bool))
        {
            if (bool.TryParse(setting.Value, out var boolValue))
            {
                return (T)Convert.ChangeType(boolValue, typeof(T));
            }
        }
        else
        {
            try
            {
                // Deserialize JSON value for generic type T
                return JsonConvert.DeserializeObject<T>(setting.Value);
            }
            catch (Exception )
            {
               
                return default;
            }
        }

        // For other types or JSON values, return null for now
        return default;
    }
}