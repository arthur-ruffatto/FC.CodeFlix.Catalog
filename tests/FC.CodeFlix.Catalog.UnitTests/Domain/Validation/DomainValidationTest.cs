using Bogus;
using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Domain.Validation;
using FluentAssertions;

namespace FC.CodeFlix.Catalog.UnitTests.Domain.Validation;

public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    [Fact(DisplayName = nameof(ValidateWhenNameIsNotNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void ValidateWhenNameIsNotNull()
    {
        var value = Faker.Commerce.ProductName();
        var fieldName = Faker.Database.Column().Replace(" ", "");
        
        Action action = () => DomainValidation.NotNull(value, fieldName);

        action.Should().NotThrow();
    }
    
    [Fact(DisplayName = nameof(ValidateWhenNameIsNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void ValidateWhenNameIsNull()
    {
        string? value = null;
        var fieldName = Faker.Database.Column().Replace(" ", "");
        
        Action action = () => DomainValidation.NotNull(value, fieldName);

        action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be null");
    }
    
    [Theory(DisplayName = nameof(ValidateWhenNameIsNullOrEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void ValidateWhenNameIsNullOrEmpty(string? target)
    {
        string? value = target;
        var fieldName = Faker.Database.Column().Replace(" ", "");
        
        Action action = () => DomainValidation.NotNullOrEmpty(value, fieldName);

        action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be null or empty");
    }
    
    [Fact(DisplayName = nameof(ValidateWhenNameIsNotNullOrEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void ValidateWhenNameIsNotNullOrEmpty()
    {
        string? value = Faker.Commerce.ProductName();
        var fieldName = Faker.Database.Column().Replace(" ", "");
        
        Action action = () => DomainValidation.NotNullOrEmpty(value, fieldName);

        action.Should().NotThrow();
    }
    
    [Theory(DisplayName = nameof(ValidateWhenNameIsLessThanMinLength))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesWithLessThanMinLength), parameters: 10)]
    public void ValidateWhenNameIsLessThanMinLength(string target, int minLength)
    {
        string value = target;
        var fieldName = Faker.Database.Column().Replace(" ", "");
        
        Action action = () => DomainValidation.MinLength(value, minLength, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be at least {minLength} characters long");
    }
    
    [Theory(DisplayName = nameof(ValidateWhenMinLengthIsOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterOrEqualToMinLength), parameters: 10)]
    public void ValidateWhenMinLengthIsOk(string target, int minLength)
    {
        string value = target;
        var fieldName = Faker.Database.Column().Replace(" ", "");
        
        Action action = () => DomainValidation.MinLength(value, minLength, fieldName);

        action.Should().NotThrow();
    }
    
    [Theory(DisplayName = nameof(ValidateWhenNameIsGreaterThanMaxLength))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesWithMoreThanMaxLength), parameters: 10)]
    public void ValidateWhenNameIsGreaterThanMaxLength(string target, int maxLength)
    {
        string value = target;
        var fieldName = Faker.Database.Column().Replace(" ", "");
        
        Action action = () => DomainValidation.MaxLength(value, maxLength, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be longer than {maxLength} characters");
    }
    
    [Theory(DisplayName = nameof(ValidateWhenMaxLengthIsOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessOrEqualToMaxLength), parameters: 10)]
    public void ValidateWhenMaxLengthIsOk(string target, int maxLength)
    {
        string value = target;
        var fieldName = Faker.Database.Column().Replace(" ", "");
        
        Action action = () => DomainValidation.MaxLength(value, maxLength, fieldName);

        action.Should().NotThrow();
    }
    
    #region STATIC METHODS

    public static IEnumerable<object[]> GetValuesWithLessThanMinLength(int numberOfTests)
    {
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var value = faker.Commerce.ProductName();
            var minLength = value.Length + new Random().Next(1, 20);
            yield return new object[] {value, minLength};
        }
    }
    
    public static IEnumerable<object[]> GetValuesGreaterOrEqualToMinLength(int numberOfTests)
    {
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var value = faker.Commerce.ProductName();
            var minLength = value.Length - new Random().Next(1, 5);
            yield return new object[] {value, minLength};
        }
    }
    
    public static IEnumerable<object[]> GetValuesWithMoreThanMaxLength(int numberOfTests)
    {
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var value = faker.Commerce.ProductName();
            var maxLength = value.Length - new Random().Next(1, 5);
            yield return new object[] {value, maxLength};
        }
    }
    
    public static IEnumerable<object[]> GetValuesLessOrEqualToMaxLength(int numberOfTests)
    {
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var value = faker.Commerce.ProductName();
            var maxLength = value.Length + new Random().Next(1, 5);
            yield return new object[] {value, maxLength};
        }
    }


    #endregion
}