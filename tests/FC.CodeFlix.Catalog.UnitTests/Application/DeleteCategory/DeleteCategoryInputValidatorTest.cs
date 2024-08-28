using FC.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
using FluentAssertions;

namespace FC.CodeFlix.Catalog.UnitTests.Application.DeleteCategory;

public class DeleteCategoryInputValidatorTest
{
    [Fact(DisplayName = nameof(InputValidatorWhenInputIsValid))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public void InputValidatorWhenInputIsValid()
    {
        var validInput = new DeleteCategoryInput(Guid.NewGuid());
        var categoryValidator = new DeleteCategoryInputValidator();

        var validatorResult = categoryValidator.Validate(validInput);
        
        validatorResult.Should().NotBeNull();
        validatorResult.IsValid.Should().BeTrue();
        validatorResult.Errors.Should().BeEmpty();
    }
    
    [Fact(DisplayName = nameof(InputValidatorWhenInputIsEmpty))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public void InputValidatorWhenInputIsEmpty()
    {
        var invalidInput = new DeleteCategoryInput(Guid.Empty);
        var categoryValidator = new DeleteCategoryInputValidator();

        var validatorResult = categoryValidator.Validate(invalidInput);
        
        validatorResult.Should().NotBeNull();
        validatorResult.IsValid.Should().BeFalse();
        validatorResult.Errors.Should().HaveCountGreaterThan(0);
        validatorResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
}