using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Domain.SeedWork;
using FC.CodeFlix.Catalog.Domain.Validation;

namespace FC.CodeFlix.Catalog.Domain.Entity;

public class Category : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Category(string name, string description, bool isActive = true) : base()
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;
        
        Validate();
    }
    
    public void ActivateCategory()
    {
        IsActive = true;
        Validate();
    }
    
    public void DeactivateCategory()
    {
        IsActive = false;
        Validate();
    }

    public void UpdateCategory(string name, string? description = null)
    {
        Name = name;
        Description = description ?? Description;
        Validate();
    }
    
    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        DomainValidation.MinLength(Name, 3, nameof(Name));
        DomainValidation.MaxLength(Name, 255, nameof(Name));
        
        DomainValidation.NotNull(Description, nameof(Description));
        DomainValidation.MaxLength(Description, 10_000, nameof(Description));
    }
}