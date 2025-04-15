using Boilerplate.Application.Auth;
using Boilerplate.Domain.Auth.Interfaces;
using Boilerplate.Domain.Interfaces;
using Boilerplate.Infrastructure;
using Boilerplate.Infrastructure.MongoDB;
using Boilerplate.Infrastructure.RabbitMq;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Boilerplate.Api.Configurations;

public static class PersistenceSetup
{
    public static IServiceCollection AddPersistenceSetup(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<ISession, Session>();
        services.AddDbContextPool<ApplicationDbContext>(o =>
        {
            o.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            o.UseExceptionProcessor();
        });


        var mongoSettings = configuration.GetSection("MongoDB").Get<MongoDbSettings>();
        services.AddSingleton<MongoDBConnection>(s => new MongoDBConnection(mongoSettings!.ConnectionString, mongoSettings!.DatabaseName));

        var rabittMQSettings = configuration.GetSection("RabbitMQ").Get<RabbitMqSettings>();
        services.AddSingleton<RabbitMQConnection>(s => new RabbitMQConnection(rabittMQSettings!.HostName, rabittMQSettings!.UserName, rabittMQSettings!.Password, rabittMQSettings!.Port));

        
      

        return services;
    }

    record MongoDbSettings(string ConnectionString, string DatabaseName);



    public record RabbitMqSettings
    {
        public string HostName { get; init; } = string.Empty;   
        public string UserName { get; init; } = string.Empty;   
        public string Password { get; init; } = string.Empty;
        public int Port { get; init; }

    }
}