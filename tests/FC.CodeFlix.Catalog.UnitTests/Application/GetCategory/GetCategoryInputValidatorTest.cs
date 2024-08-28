using FC.CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
using FluentAssertions;

namespace FC.CodeFlix.Catalog.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryInputValidatorTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryInputValidatorTest(GetCategoryTestFixture fixture) 
        => _fixture = fixture;
    

    [Fact(DisplayName = nameof(GetCategoryValidatorWhenInputIsValid))]
    [Trait("Application", "GetCategory - Use Cases")]
    public void GetCategoryValidatorWhenInputIsValid()
    {
        var validInput = new GetCategoryInput(Guid.NewGuid());
        var categoryValidator = new GetCategoryInputValidator();

        var validatorResult = categoryValidator.Validate(validInput);
        
        validatorResult.Should().NotBeNull();
        validatorResult.IsValid.Should().BeTrue();
        validatorResult.Errors.Should().BeEmpty();
    }
    
    [Fact(DisplayName = nameof(GetCategoryValidatorWhenInputIsEmpty))]
    [Trait("Application", "GetCategory - Use Cases")]
    public void GetCategoryValidatorWhenInputIsEmpty()
    {
        var invalidInput = new GetCategoryInput(Guid.Empty);
        var categoryValidator = new GetCategoryInputValidator();

        var validatorResult = categoryValidator.Validate(invalidInput);
        
        validatorResult.Should().NotBeNull();
        validatorResult.IsValid.Should().BeFalse();
        validatorResult.Errors.Should().HaveCountGreaterThan(0);
        validatorResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
}