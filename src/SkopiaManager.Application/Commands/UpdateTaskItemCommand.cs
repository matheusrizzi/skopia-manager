using System.Diagnostics.CodeAnalysis;
using MediatR;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Enums;

namespace SkopiaManager.Application.Commands;

[ExcludeFromCodeCoverage]
public record UpdateTaskItemCommand(
    int TaskItemId,
    string? Title,
    string? Description,
    DateTime? DueDate,
    TaskStatusEnum? Status,
    int userId
) : IRequest<TaskItem>;
