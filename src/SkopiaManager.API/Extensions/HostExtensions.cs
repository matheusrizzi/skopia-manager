using Microsoft.EntityFrameworkCore;
using SkopiaManager.Infrastructure.Data;

namespace SkopiaManager.API.Extensions;

public static class HostExtensions
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SkopiaDbContext>();

        db.Database.Migrate();

        return host;
    }
}
