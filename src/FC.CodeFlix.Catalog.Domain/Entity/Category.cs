using FC.CodeFlix.Catalog.Domain.Exceptions;

namespace FC.CodeFlix.Catalog.Domain.Entity;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Category(string name, string description, bool isActive = true)
    {
        Id = Guid.NewGuid();
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
        if (String.IsNullOrWhiteSpace(Name))
            throw new EntityValidationException($"{nameof(Name)} should not be null or empty");
        
        if (Name.Length < 3)
            throw new EntityValidationException($"{nameof(Name)} should be at least 3 characters long");
        
        if (Name.Length > 255)
            throw new EntityValidationException($"{nameof(Name)} should not be longer than 255 characters");
        
        if (Description == null)
            throw new EntityValidationException($"{nameof(Description)} should not be null or empty");
        
        if (Description.Length > 10_000)
            throw new EntityValidationException($"{nameof(Description)} should not be longer than 10.000 characters");
    }
}