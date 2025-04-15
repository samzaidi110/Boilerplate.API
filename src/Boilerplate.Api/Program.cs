using Boilerplate.Api.Common;
using Boilerplate.Api.Configurations;
using Boilerplate.Api.Endpoints;
using Boilerplate.Domain.Auth;
using Boilerplate.Domain.Interfaces;
using Boilerplate.Infrastructure;
using Boilerplate.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddJwtPolicySetup();
// Controllers
builder.AddValidationSetup();

builder.Services.AddAuthorization();

//builder.Services.AddScoped<IUserAccessor, UserAccessor>();


// Swagger
builder.Services.AddSwaggerSetup();

// Persistence
builder.Services.AddPersistenceSetup(builder.Configuration);

// Application layer setup
builder.Services.AddApplicationSetup();



// Request response compression
builder.Services.AddCompressionSetup();

builder.Services.AddHttpContextAccessor();

// Mediator
builder.Services.AddMediatRSetup();

// Exception handler
builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Logging.ClearProviders();

// Add serilog
if (builder.Environment.EnvironmentName != "Testing")
{
    builder.Host.UseLoggingSetup(builder.Configuration);
    
    // Add opentelemetry
    builder.AddOpenTemeletrySetup();
}

#region sample http
builder.Services.AddHttpService();

builder.AddHttpServiceSetting();

#endregion

var app = builder.Build();



// Configure the HTTP request pipeline.
app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseSwaggerSetup(app.Environment.IsDevelopment());

app.UseHsts();

app.UseResponseCompression();
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseMiddleware<LoggingMiddleware>();



app.UseAuthorization();




app.MapHeroEndpoints();
app.MapHttpEndpoints();
app.MapBookEndpoints();
app.MapAuthorsEndpoints();
await app.Migrate();

await app.RunAsync();