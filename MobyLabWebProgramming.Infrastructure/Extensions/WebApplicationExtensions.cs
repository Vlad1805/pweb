using Microsoft.AspNetCore.Builder;
using MobyLabWebProgramming.Infrastructure.Middlewares;
using Serilog;

namespace MobyLabWebProgramming.Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication application)
    {
        application.UseMiddleware<GlobalExceptionHandlerMiddleware>()
            .UseSwagger()
            .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DataService v1"))
            .UseCors()
            .UseRouting()
            .UseAuthentication()
            .UseSerilogRequestLogging()
            .UseAuthorization();
        application.MapControllers();

        return application;
    }
}
