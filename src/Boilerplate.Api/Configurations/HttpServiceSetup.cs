using Boilerplate.Domain.Auth;
using Boilerplate.Domain.Auth.Interfaces;
using Boilerplate.Infrastructure.Http;
using Boilerplate.Infrastructure.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Boilerplate.Api.Configurations;

public static class HttpServiceSetup
{
    public static IServiceCollection AddHttpService(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<IHttpEmployeeService, HttpEmployeeService>();

        return services;
    }

    public static void AddHttpServiceSetting(this WebApplicationBuilder builder)
    {
        var httpServiceSetting = builder.Configuration.GetSection("HttpService");

        builder.Services.Configure<HttpServiceSetting>(httpServiceSetting);

        builder.Services.AddHttpClient<DataHttpClientService>().AddHttpMessageHandler<OutgoingLoggingHandler>();

    }

    
}