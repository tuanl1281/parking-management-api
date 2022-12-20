using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Parking.Management.Application.Middlewares;
using Parking.Management.Application.Extensions;

namespace Parking.Management.Application;

public class Startup
{
    private IConfiguration Configuration { get; }
    
    private IWebHostEnvironment Environment { get; }

    public Startup(IWebHostEnvironment environment)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        Environment = environment;
        Configuration = builder.Build();
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        #region --- Versioning ---
        services.AddApiVersioning(_ =>
        {
            _.DefaultApiVersion = new ApiVersion(1, 0);
            _.AssumeDefaultVersionWhenUnspecified = true;
            _.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(_ =>
        {
            _.GroupNameFormat = "'v'VVV";
            _.SubstituteApiVersionInUrl = true;
        });
        #endregion
        
        /* For request body size */
        services.Configure<IISServerOptions>(options =>  options.MaxRequestBodySize = int.MaxValue);
        services.Configure<KestrelServerOptions>(options => options.Limits.MaxRequestBodySize = int.MaxValue);
        /* For configuration */
        services.AddSettings(Configuration);
        /* For controller */
        services.AddControllers();
        /* For policy */
        services.AddCorsPolicy();
        /* For JWT */
        services.AddJwt(Configuration["Jwt:Key"], Configuration["Jwt:Issuer"], Configuration["Jwt:Issuer"]);
        /* For swagger */
        services.AddSwagger(Environment, Configuration);
        /* For mapper */  
        services.AddMapper();
        /* For accessor */
        services.AddHttpContextAccessor();
        /* For layer pattern */
        services.AddRepositories();
        services.AddBusinessServices();
    }
    
    public void Configure(IApplicationBuilder application, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
            application.UseDeveloperExceptionPage();
        /* Policy */
        application.UseCors("AllowAll");
        /* Middleware */
        application.UseMiddleware<ErrorHandlerMiddleware>();
        /* For route */
        application.UseHttpsRedirection();
        application.UseRouting();
        /* For authentication */
        application.UseAuthentication();
        application.UseAuthorization();
        /* For static file */
        application.UseStaticFiles(
            new StaticFileOptions
            {
                OnPrepareResponse = _ =>
                {
                    if (_.Context.Request.Path.StartsWithSegments("/files"))
                    {
                        _.Context.Response.Headers.Add("Cache-Control", "no-store");
                        if (_.Context?.User?.Identity != null && !_.Context.User.Identity.IsAuthenticated)
                        {
                            _.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            _.Context.Response.ContentLength = 0;
                            _.Context.Response.Body = Stream.Null;
                            _.Context.Response.Redirect("/");
                        }
                    }
                }
            }
        );
        /* For endpoint */
        application.UseEndpoints(_ =>  _.MapControllers());
        /* For swagger */
        application.UseOpenApi();
        application.UseSwaggerUi3(_ => _.CustomStylesheetPath = "/styles/swagger.css");
    }
}
