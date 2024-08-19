using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Repository;
using Moq;
using UseCases = FC.CodeFlix.Catalog.Application.UseCases.CreateCategory;

namespace FC.CodeFlix.Catalog.UnitTests.Application.CreateCategory;

public class CreateCategoryTest
{
    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var input = new CreateCategoryInput("Category Name", "Category Description", true);
        var output = await useCase.Handle(input, CancellationToken.None);
        
        repositoryMock.Verify(repository => repository
            .Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), 
            Times.Once());
        
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once());

        output.Should().NotBeNull();
        output.Name.Should().Be("Category Name");
        output.Description.Should().Be("Category Description");
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeNullOrEmpty();
    }
}