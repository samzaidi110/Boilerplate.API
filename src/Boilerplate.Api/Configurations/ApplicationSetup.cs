using Boilerplate.Application.Common;
using Boilerplate.Application.Features.Logging;
using Boilerplate.Domain.Entities.Logging;
using Boilerplate.Domain.Interfaces;
using Boilerplate.Infrastructure;
using Boilerplate.Infrastructure.Authorization;
using Boilerplate.Infrastructure.Logging;
using Boilerplate.Infrastructure.MongoDB;
using Boilerplate.Infrastructure.Postgres;
using Boilerplate.Infrastructure.RabbitMq;
using Boilerplate.Infrastructure.Setting;
using Boilerplate.Persistence.Repositories;
using Google.Protobuf.WellKnownTypes;
using MassTransit;
using MassTransit.NewIdProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Api.Configurations;

public static class ApplicationSetup
{
    public static IServiceCollection AddApplicationSetup(this IServiceCollection services)
    {
        
        services.AddScoped<IContext, ApplicationDbContext>();
        NewId.SetProcessIdProvider(new CurrentProcessIdProvider());

        services.AddScoped<IHeroDocumentRepository, HeroMongoRepository>();
        services.AddScoped<IBookDocumentRepository, BookDocumentRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBookGenreRepository, BookGenreRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddScoped<IRabbitMQService, RabbitMQService>();

        services.AddScoped<IOperationScoped, OperationScoped>();
        services.AddScoped<LoggingMiddleware>();
       

       
        services.AddTransient<OutgoingLoggingHandler>();

        services.AddScoped<IGlobalSettingService, SystemSettingRepository>();

        

        return services;
    }
    
    
}