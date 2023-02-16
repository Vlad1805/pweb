using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Workers;

public class InitializerWorker : BackgroundService
{
    private readonly ILogger<InitializerWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public InitializerWorker(ILogger<InitializerWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var userService = scope.ServiceProvider.GetService<IUserService>();

            if (userService == null)
            {
                _logger.LogInformation("Couldn't create the user service!");

                return;
            }

            var count = await userService.GetUserCount(cancellationToken);

            if (count.Result == 0)
            {
                _logger.LogInformation("No user found, adding default user!");

                await userService.AddUser(new()
                {
                    Email = "admin@default.com",
                    Name = "Admin",
                    Role = UserRoleEnum.Admin,
                    Password = PasswordUtils.HashPassword("default")
                }, cancellationToken: cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing database!");
        }
    }
}