﻿using FC.CodeFlix.Catalog.Application.Interfaces;
using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Repository;
using FC.CodeFlix.Catalog.UnitTests.Common;
using Moq;

namespace FC.CodeFlix.Catalog.UnitTests.Application.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection 
    : ICollectionFixture<DeleteCategoryTestFixture> { }

public class DeleteCategoryTestFixture : BaseFixture
{
    public DeleteCategoryTestFixture() : base() { }
    
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
    
    public Category GetValidCategory()
        => new (GetValidCategoryName(), GetValidCategoryDescription());
}