using FC.CodeFlix.Catalog.Application.Interfaces;
using FC.CodeFlix.Catalog.Application.UseCases.Category;
using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Domain.Repository;
using FluentAssertions;
using Moq;
using UseCases = FC.CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.CodeFlix.Catalog.UnitTests.Application.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{

    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture createCategoryTestFixture)
    {
        _fixture = createCategoryTestFixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {

        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object, 
            unitOfWorkMock.Object
            );

        var input = _fixture.GetValidInput();
        var output = await useCase.Handle(input, CancellationToken.None);
        
        repositoryMock.Verify(repository => repository
            .Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), 
            Times.Once());
        
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once());

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }
    
    [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlyName()
    {

        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object, 
            unitOfWorkMock.Object
        );

        var input = new UseCases.CreateCategoryInput(_fixture.GetValidCategoryName());
        var output = await useCase.Handle(input, CancellationToken.None);
        
        repositoryMock.Verify(repository => repository
                .Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), 
            Times.Once());
        
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once());

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be("");
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }
    
    [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlyNameAndDescription()
    {

        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object, 
            unitOfWorkMock.Object
        );

        var input = new UseCases.CreateCategoryInput(
            _fixture.GetValidCategoryName(), 
            _fixture.GetValidCategoryDescription()
            );
        
        var output = await useCase.Handle(input, CancellationToken.None);
        
        repositoryMock.Verify(repository => repository
                .Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), 
            Times.Once());
        
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once());

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ValidateWhenInputIsInvalid))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(
        CreateCategoryTestDataGenerator.GetInvalidInputs), parameters: 12, 
        MemberType = typeof(CreateCategoryTestDataGenerator)
        )
    ]
    public async void ValidateWhenInputIsInvalid(UseCases.CreateCategoryInput input, string exceptionMessage)
    {
        var useCase = new UseCases.CreateCategory(
            _fixture.GetRepositoryMock().Object, 
            _fixture.GetUnitOfWorkMock().Object
        );
        
        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);
    }
}