using FC.CodeFlix.Catalog.Application.Interfaces;
using FC.CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.CodeFlix.Catalog.Domain.Repository;
using FC.CodeFlix.Catalog.UnitTests.Common;
using Moq;

namespace FC.CodeFlix.Catalog.UnitTests.Application.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CategoryTestFixtureCollection 
    : ICollectionFixture<CreateCategoryTestFixture>
{
    
}

public class CreateCategoryTestFixture : BaseFixture
{

    public CreateCategoryTestFixture() : base() { }
    
    public string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName[..255];
        
        return categoryName;
    }
    
    public string GetValidCategoryDescription()
    {
        var categoryDescription = "";

        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];
        
        return categoryDescription;
    }
    
    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;

    public CreateCategoryInput GetValidInput()
        => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());

    public CreateCategoryInput GetNameTooShort()
    {
        var invalidName = GetValidInput();
        invalidName.Name = invalidName.Name[..2];
        return invalidName;
    }
    
    public CreateCategoryInput GetNameTooLong()
    {
        var invalidName = GetValidInput();
        invalidName.Name = Faker.Commerce.ProductName();
        while (invalidName.Name.Length <= 255)
            invalidName.Name += Faker.Commerce.ProductName();
        return invalidName;
    }

    public CreateCategoryInput GetNullDescription()
    {
        var invalidDescription = GetValidInput();
        invalidDescription.Description = null!;
        return invalidDescription;
    }
    
    public CreateCategoryInput GetDescriptionTooLong()
    {
        var invalidDescription = GetValidInput();
        invalidDescription.Description = Faker.Commerce.ProductDescription();
        while (invalidDescription.Description.Length <= 10_000)
            invalidDescription.Description += Faker.Commerce.ProductDescription();
        return invalidDescription;
    }
    
    public Mock<ICategoryRepository> GetRepositoryMock() => new() ;
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();

}