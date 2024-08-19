namespace FC.CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;

public class CreateCategoryInput
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    
    public CreateCategoryInput(string name, string description, bool isActive)
    {
        Name = name;
        Description = description ?? "";
        IsActive = isActive;
    }
}