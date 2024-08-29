using FC.CodeFlix.Catalog.Domain.Entity;
using FluentAssertions;
using Moq;
using UseCases = FC.CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture) 
        => _fixture = fixture;

    [Theory(DisplayName = nameof(TestUpdateCategory))]
    [Trait("Application ", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), 
        parameters: 10,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
        )
    ]
    public async Task TestUpdateCategory(Category category, UseCases.UpdateCategoryInput input)
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        repositoryMock.Setup(x => x.Get(
                category.Id, 
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(category);
        var useCase = new UseCases.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        
        //Act
        var output = await useCase.Handle(input, CancellationToken.None);
        
        //Assert
        repositoryMock.Verify(x => x.Get(
                category.Id, 
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
        repositoryMock.Verify(x => x.Update(
                category, 
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
        unitOfWorkMock.Verify(x => x.Commit(
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
        
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().Be(category.Id);
        output.CreatedAt.Should().Be(category.CreatedAt);
    }
}