using Microsoft.Extensions.DependencyInjection;
using SkopiaManager.Application.Interfaces;
using SkopiaManager.Domain.Interfaces;
using SkopiaManager.Infrastructure.Repositories;

namespace SkopiaManager.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Repositórios
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITaskItemRepository, TaskItemRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IChangeLogRepository, ChangeLogRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
