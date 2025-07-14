using System.Diagnostics.CodeAnalysis;
using MediatR;
using SkopiaManager.Application.Dtos;
using SkopiaManager.Domain.Enums;

namespace SkopiaManager.Application.Commands;

[ExcludeFromCodeCoverage]
public record CreateTaskItemCommand(
    string Title,
    string? Description,
    DateTime? DueDate,
    int ProjectId,
    PriorityEnum Priority,
    int UserId
) : IRequest<TaskItemDto>;
