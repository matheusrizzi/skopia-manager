using System.Diagnostics.CodeAnalysis;

namespace SkopiaManager.Domain.Entities;

[ExcludeFromCodeCoverage]
public class User
{
    public int Id { get; set; }  
    public string Name { get; set; }
    public string Role { get; set; }

    public ICollection<Project> Projects { get; set; } = new List<Project>();
}