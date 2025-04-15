using Boilerplate.Infrastructure;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;
using Boilerplate.Domain.Auth;
using EntityFramework.Exceptions.PostgreSQL;
using Boilerplate.Infrastructure.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Api.Configurations;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();


        var config = GetAppConfiguration();
        optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        optionsBuilder.UseExceptionProcessor();

        var services= new ServiceCollection();
        var serviceprovider= services.BuildServiceProvider();

        return new ApplicationDbContext(optionsBuilder.Options, serviceprovider);
    }


  static IConfiguration GetAppConfiguration()
    {
        var environmentName =
            Environment.GetEnvironmentVariable(
                "ASPNETCORE_ENVIRONMENT");

        var dir = Directory.GetParent(AppContext.BaseDirectory);
        
        if (Microsoft.Extensions.Hosting.Environments.Development.Equals(environmentName,
                StringComparison.OrdinalIgnoreCase))
        {
            var depth = 0;
            do
                dir = dir!.Parent;
            while (++depth < 5 && dir!.Name != "bin");
            dir = dir!.Parent;
        }

        var path = dir!.FullName;

        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environmentName}.json", true)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
