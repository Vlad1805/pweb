using Ardalis.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;

namespace MobyLabWebProgramming.Infrastructure.Database;

public sealed class WebAppDatabaseContext : DbContext
{
    public WebAppDatabaseContext(DbContextOptions<WebAppDatabaseContext> options, bool migrate = true) : base(options)
    {
        if (migrate)
        {
            Database.Migrate();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("unaccent")
            .ApplyAllConfigurationsFromCurrentAssembly();
        modelBuilder.ConfigureSmartEnum();
    }
}