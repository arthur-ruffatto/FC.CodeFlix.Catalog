using FC.CodeFlix.Catalog.Domain.Exceptions;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        /*TDD*/
        //Arrange
        var validData = new
        {
            Name = "Category name",
            Description = "Category description"
        };
        
        //Act
        var dateTimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var dateTimeAfter = DateTime.Now;
        
        //Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.True(category.IsActive);
    }
    
    [Theory(DisplayName = nameof(InstantiateWithActiveStatus))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithActiveStatus(bool isActive)
    {
        /*TDD*/
        //Arrange
        var validData = new
        {
            Name = "Category name",
            Description = "Category description",
            IsActive = isActive
        };
        
        //Act
        var dateTimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validData.Name, validData.Description, validData.IsActive);
        var dateTimeAfter = DateTime.Now;
        
        //Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWhenNameIsNullOrEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void InstantiateWhenNameIsNullOrEmpty(string? name)
    {
        Action action = () => new DomainEntity.Category(name!, "Category description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be null or empty", exception.Message);
    }
    
    [Fact(DisplayName = nameof(InstantiateWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateWhenDescriptionIsNull()
    {
        Action action = () => new DomainEntity.Category("Category name", null!);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be null or empty", exception.Message);
    }
    
    [Theory(DisplayName = nameof(InstantiateWhenNameIsLessThan3Chars))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("1")]
    [InlineData("12")]
    public void InstantiateWhenNameIsLessThan3Chars(string invalidName)
    {
        Action action = () => new DomainEntity.Category(invalidName, "Category description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at least 3 characters long", exception.Message);
    }
    
    [Fact(DisplayName = nameof(InstantiateWhenNameIsMoreThan255Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateWhenNameIsMoreThan255Chars()
    {
        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a"));
        Action action = () => new DomainEntity.Category(invalidName, "Category description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be longer than 255 characters", exception.Message);
    }
    
    [Fact(DisplayName = nameof(InstantiateWhenDescriptionIsMoreThan10_000Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateWhenDescriptionIsMoreThan10_000Chars()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(0, 10_001).Select(_ => "a"));
        Action action = () => new DomainEntity.Category("Name", invalidDescription);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be longer than 10.000 characters", exception.Message);
    }
    
    [Fact(DisplayName = nameof(ActivateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void ActivateCategory()
    {
        /*TDD*/
        //Arrange
        var validData = new
        {
            Name = "Category name",
            Description = "Category description",
            IsActive = false
        };
        
        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, validData.IsActive);
        category.ActivateCategory();
        
        //Assert
        Assert.True(category.IsActive);
    }
    
    [Fact(DisplayName = nameof(DeactivateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void DeactivateCategory()
    {
        /*TDD*/
        //Arrange
        var validData = new
        {
            Name = "Category name",
            Description = "Category description",
            IsActive = true
        };
        
        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, validData.IsActive);
        category.DeactivateCategory();
        
        //Assert
        Assert.False(category.IsActive);
    }
    
    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateCategory()
    {
        /*TDD*/
        //Arrange
        var category = new DomainEntity.Category("Category name", "Category description");
        var newData = new { Name = "New category name", Description = "New category description" };
        
        //Act
        category.UpdateCategory(newData.Name, newData.Description);
        
        //Assert
        Assert.Equal(newData.Name, category.Name);
        Assert.Equal(newData.Description, category.Description);
    }
    
    [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateCategoryOnlyName()
    {
        /*TDD*/
        //Arrange
        var category = new DomainEntity.Category("Category name", "Category description");
        var newData = new { Name = "New category name" };
        var currentDescription = category.Description;
        
        //Act
        category.UpdateCategory(newData.Name);
        
        //Assert
        Assert.Equal(newData.Name, category.Name);
        Assert.Equal(currentDescription, category.Description);
    }
    
    [Theory(DisplayName = nameof(UpdateCategoryWhenNameIsNullOrEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateCategoryWhenNameIsNullOrEmpty(string? name)
    {
        var category = new DomainEntity.Category("Category name", "Category description");
        Action action = () => category.UpdateCategory(name!, "Category description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be null or empty", exception.Message);
    }
    
    [Theory(DisplayName = nameof(UpdateCategoryWhenNameIsLessThan3Chars))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("1")]
    [InlineData("12")]
    public void UpdateCategoryWhenNameIsLessThan3Chars(string invalidName)
    {
        var category = new DomainEntity.Category("Category name", "Category description");
        Action action = () => category.UpdateCategory(invalidName, "Category description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at least 3 characters long", exception.Message);
    }
    
    [Fact(DisplayName = nameof(UpdateCategoryWhenNameIsMoreThan255Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateCategoryWhenNameIsMoreThan255Chars()
    {
        var category = new DomainEntity.Category("Category name", "Category description");
        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a"));
        Action action = () => category.UpdateCategory(invalidName, "Category description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be longer than 255 characters", exception.Message);
    }
    
    [Fact(DisplayName = nameof(UpdateCategoryWhenDescriptionIsMoreThan10_000Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateCategoryWhenDescriptionIsMoreThan10_000Chars()
    {
        var category = new DomainEntity.Category("Category name", "Category description");
        var invalidDescription = String.Join(null, Enumerable.Range(0, 10_001).Select(_ => "a"));
        Action action = () => category.UpdateCategory(category.Name, invalidDescription);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be longer than 10.000 characters", exception.Message);
    }
}