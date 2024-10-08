﻿using FC.CodeFlix.Catalog.Application.Interfaces;
using FC.CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Repository;
using FC.CodeFlix.Catalog.UnitTests.Common;
using Moq;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture> { }
public class UpdateCategoryTestFixture : BaseFixture
{
    public Mock<ICategoryRepository> GetRepositoryMock() => new() ;
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
    
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

    public Category GetValidCategory()
        => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());

    public UpdateCategoryInput GetValidInput(Guid? id = null)
    {
        return new UpdateCategoryInput(
            id ?? Guid.NewGuid(), 
            GetValidCategoryName(), 
            GetValidCategoryDescription(), 
            GetRandomBoolean()
            );
    }
    
    public UpdateCategoryInput GetNameTooShort()
    {
        var invalidName = GetValidInput();
        invalidName.Name = invalidName.Name[..2];
        return invalidName;
    }
    
    public UpdateCategoryInput GetNameTooLong()
    {
        var invalidName = GetValidInput();
        invalidName.Name = Faker.Commerce.ProductName();
        while (invalidName.Name.Length <= 255)
            invalidName.Name += Faker.Commerce.ProductName();
        return invalidName;
    }
    
    public UpdateCategoryInput GetDescriptionTooLong()
    {
        var invalidDescription = GetValidInput();
        invalidDescription.Description = Faker.Commerce.ProductDescription();
        while (invalidDescription.Description.Length <= 10_000)
            invalidDescription.Description += Faker.Commerce.ProductDescription();
        return invalidDescription;
    }
}