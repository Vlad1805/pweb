namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This is an abstract class that serves as a base for other classes in the project.
/// Other classes in the project can inherit from this BaseEntity class and use its properties to
/// uniquely identify and track the creation and modification times of their own entities.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// The Id property is a unique identifier for each entity, generated as a GUID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The CreatedAt property represents the date and time when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The UpdatedAt property represents the date and time when the entity was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// This method updates the UpdatedAt property to the current date and time in UTC.
    /// </summary>
    public void UpdateTime() => UpdatedAt = DateTime.UtcNow;
}