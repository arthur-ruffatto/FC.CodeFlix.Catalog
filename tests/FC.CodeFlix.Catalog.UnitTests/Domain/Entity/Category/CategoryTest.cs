using FC.CodeFlix.Catalog.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;
    
    public CategoryTest(CategoryTestFixture categoryTestFixture) 
        => _categoryTestFixture = categoryTestFixture;

    #region STATIC METHODS

    public static IEnumerable<object[]> GetNamesWithLessThan3Chars(int numberOfNames)
    {
        var fixture = new CategoryTestFixture();
        for (int i = 0; i < numberOfNames; i++)
        {
            var isOdd = i % 2 == 1;
            yield return new object[]
            {
                fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)]
            };
        }
    }

    #endregion

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        /*TDD*/
        //Arrange
        var validData = _categoryTestFixture.GetValidCategory();
        
        //Act
        var dateTimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var dateTimeAfter = DateTime.Now;
        
        //Assert using FluentAssertions
        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        category.CreatedAt.Should().BeAfter(dateTimeBefore);
        category.CreatedAt.Should().BeBefore(dateTimeAfter);
        category.IsActive.Should().BeTrue();
        
        /*Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.True(category.IsActive);*/
    }
    
    [Theory(DisplayName = nameof(InstantiateWithActiveStatus))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithActiveStatus(bool isActive)
    {
        /*TDD*/
        //Arrange
        var validData = _categoryTestFixture.GetValidCategory();
        
        //Act
        var dateTimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        var dateTimeAfter = DateTime.Now;
        
        //Assert using FluentAssertions
        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        category.CreatedAt.Should().BeAfter(dateTimeBefore);
        category.CreatedAt.Should().BeBefore(dateTimeAfter);
        category.IsActive.Should().Be(isActive);
        
        /*Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.Equal(isActive, category.IsActive);*/
    }

    [Theory(DisplayName = nameof(InstantiateWhenNameIsNullOrEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void InstantiateWhenNameIsNullOrEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        
        Action action = () => new DomainEntity.Category(name!, validCategory.Description);
        
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be null or empty");
    }
    
    [Fact(DisplayName = nameof(InstantiateWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        
        Action action = () => new DomainEntity.Category(validCategory.Name, null!);
        
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should not be null");
    }
    
    [Theory(DisplayName = nameof(InstantiateWhenNameIsLessThan3Chars))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNamesWithLessThan3Chars), parameters: 10)]
    public void InstantiateWhenNameIsLessThan3Chars(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        
        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
        
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be at least 3 characters long");
    }
    
    [Fact(DisplayName = nameof(InstantiateWhenNameIsMoreThan255Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateWhenNameIsMoreThan255Chars()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);
        
        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
        
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be longer than 255 characters");
    }
    
    [Fact(DisplayName = nameof(InstantiateWhenDescriptionIsMoreThan10_000Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateWhenDescriptionIsMoreThan10_000Chars()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidDescription = _categoryTestFixture.Faker.Lorem.Letter(10_001);
        
        Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);
        
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should not be longer than 10000 characters");
    }
    
    [Fact(DisplayName = nameof(ActivateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void ActivateCategory()
    {
        /*TDD*/
        //Arrange
        var validData = _categoryTestFixture.GetValidCategory();
        
        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, validData.IsActive);
        category.ActivateCategory();
        
        //Assert
        category.IsActive.Should().BeTrue();
    }
    
    [Fact(DisplayName = nameof(DeactivateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void DeactivateCategory()
    {
        /*TDD*/
        //Arrange
        var validData = _categoryTestFixture.GetValidCategory();
        
        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, validData.IsActive);
        category.DeactivateCategory();
        
        //Assert
        category.IsActive.Should().BeFalse();
    }
    
    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateCategory()
    {
        /*TDD*/
        //Arrange
        var category = _categoryTestFixture.GetValidCategory();
        var newData = _categoryTestFixture.GetValidCategory();
        
        //Act
        category.UpdateCategory(newData.Name, newData.Description);
        
        //Assert
        category.Name.Should().Be(newData.Name);
        category.Description.Should().Be(newData.Description);
    }
    
    [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateCategoryOnlyName()
    {
        /*TDD*/
        //Arrange
        var category = _categoryTestFixture.GetValidCategory();
        var newName = _categoryTestFixture.GetValidCategoryName();
        var currentDescription = category.Description;
        
        //Act
        category.UpdateCategory(newName);
        
        //Assert
        category.Name.Should().Be(newName);
        category.Description.Should().Be(currentDescription);
    }
    
    [Theory(DisplayName = nameof(UpdateCategoryWhenNameIsNullOrEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateCategoryWhenNameIsNullOrEmpty(string? name)
    {
        var category = _categoryTestFixture.GetValidCategory();
        
        Action action = () => category.UpdateCategory(name!, category.Description);
        
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be null or empty");
    }
    
    [Theory(DisplayName = nameof(UpdateCategoryWhenNameIsLessThan3Chars))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNamesWithLessThan3Chars), parameters: 10)]
    public void UpdateCategoryWhenNameIsLessThan3Chars(string invalidName)
    {
        var category = _categoryTestFixture.GetValidCategory();
        
        Action action = () => category.UpdateCategory(invalidName);
        
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be at least 3 characters long");
    }
    
    [Fact(DisplayName = nameof(UpdateCategoryWhenNameIsMoreThan255Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateCategoryWhenNameIsMoreThan255Chars()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);
        
        Action action = () => category.UpdateCategory(invalidName);
        
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be longer than 255 characters");
    }
    
    [Fact(DisplayName = nameof(UpdateCategoryWhenDescriptionIsMoreThan10_000Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateCategoryWhenDescriptionIsMoreThan10_000Chars()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var invalidDescription = _categoryTestFixture.Faker.Lorem.Letter(10_001);
        
        Action action = () => category.UpdateCategory(category.Name, invalidDescription);
        
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should not be longer than 10000 characters");
    }
}