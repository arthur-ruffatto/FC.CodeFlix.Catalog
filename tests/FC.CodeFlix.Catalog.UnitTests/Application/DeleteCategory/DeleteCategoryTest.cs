using Moq;
using UseCases = FC.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;

namespace FC.CodeFlix.Catalog.UnitTests.Application.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;
    
    public DeleteCategoryTest(DeleteCategoryTestFixture fixture) => _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategory()
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        
        var category = _fixture.GetValidCategory();
        repositoryMock.Setup(x => x.Get(
            category.Id, 
            It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(category);
        
        var input = new DeleteCategoryInput(category.Id);
        var useCase = new UseCases.DeleteCategory(repositoryMock.Object, unitOfWorkMock.Object);
        
        //Act
        await useCase.Handle(input, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.Get(
            category.Id,
            It.IsAny<CancellationToken>()
            ), Times.Once
        );
        
        repositoryMock.Verify(x => x.Delete(
                category,
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
        
        unitOfWorkMock.Verify(x => x.Commit(
                It.IsAny<CancellationToken>()
            ), Times.Once
        );


    }
}