using FC.CodeFlix.Catalog.Application.Exceptions;
using FluentAssertions;
using Moq;
using UseCase = FC.CodeFlix.Catalog.Application.UseCases.Category.GetCategory;

namespace FC.CodeFlix.Catalog.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async void GetCategory()
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var validCategory = _fixture.GetValidCategory();

        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(validCategory);

        var input = new UseCase.GetCategoryInput(validCategory.Id);
        var useCase = new UseCase.GetCategory(repositoryMock.Object);
        
        //Act
        var output = await useCase.Handle(input, CancellationToken.None);
        
        repositoryMock.Verify(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            ), Times.Once
        );
        
        //Assert
        output.Should().NotBeNull();
        output.Id.Should().Be(validCategory.Id);
        output.Name.Should().Be(validCategory.Name);
        output.Description.Should().Be(validCategory.Description);
        output.IsActive.Should().Be(validCategory.IsActive);
        output.CreatedAt.Should().Be(validCategory.CreatedAt);
    }
    
    [Fact(DisplayName = nameof(CategoryNotFoundException))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async void CategoryNotFoundException()
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var guid = Guid.NewGuid();

        repositoryMock.Setup(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            )
        ).ThrowsAsync(new NotFoundException($"Category {guid} not found"));

        var input = new UseCase.GetCategoryInput(guid);
        var useCase = new UseCase.GetCategory(repositoryMock.Object);
        
        //Act
        
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        //Assert
        await task.Should().ThrowAsync<NotFoundException>();
        
        repositoryMock.Verify(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
    }
}