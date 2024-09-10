using System.Runtime.Serialization.DataContracts;
using FC.CodeFlix.Catalog.Application.Exceptions;
using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Exceptions;
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
        output.IsActive.Should().Be((bool)input.IsActive!);
        output.Id.Should().Be(category.Id);
        output.CreatedAt.Should().Be(category.CreatedAt);
    }
    
    [Fact(DisplayName = nameof(TestUpdateWhenCategoryNotFound))]
    [Trait("Application ", "UpdateCategory - Use Cases")]
    public async Task TestUpdateWhenCategoryNotFound()
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = _fixture.GetValidInput();
        repositoryMock.Setup(x => x.Get(
                input.Id, 
                It.IsAny<CancellationToken>()
            )
        ).ThrowsAsync(new NotFoundException($"Category {input.Id} not found"));
        var useCase = new UseCases.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        
        //Act
        var task = async () => await useCase.Handle(input, CancellationToken.None);
        
        //Assert
        await task.Should().ThrowAsync<NotFoundException>();
        
        repositoryMock.Verify(x => x.Get(
                input.Id, 
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
    }
    
    [Theory(DisplayName = nameof(TestUpdateCategoryWhenNotProvidingIsActive))]
    [Trait("Application ", "UpdateCategory - Use Cases")]
    [MemberData(
            nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), 
            parameters: 10,
            MemberType = typeof(UpdateCategoryTestDataGenerator)
        )
    ]
    public async Task TestUpdateCategoryWhenNotProvidingIsActive(Category category, UseCases.UpdateCategoryInput input)
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var inputWithoutIsActive = new UseCases.UpdateCategoryInput(input.Id, input.Name, input.Description);
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
        output.Name.Should().Be(inputWithoutIsActive.Name);
        output.Description.Should().Be(inputWithoutIsActive.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);
        output.Id.Should().Be(category.Id);
        output.CreatedAt.Should().Be(category.CreatedAt);
    }
    
    [Theory(DisplayName = nameof(TestUpdateCategoryWhenNotProvidingIsActive))]
    [Trait("Application ", "UpdateCategory - Use Cases")]
    [MemberData(
            nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), 
            parameters: 10,
            MemberType = typeof(UpdateCategoryTestDataGenerator)
        )
    ]
    public async Task TestUpdateCategoryProvidingOnlyName(Category category, UseCases.UpdateCategoryInput input)
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var inputWithoutIsActive = new UseCases.UpdateCategoryInput(input.Id, input.Name);
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
        output.Name.Should().Be(inputWithoutIsActive.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);
        output.Id.Should().Be(category.Id);
        output.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Theory(DisplayName = nameof(TestWhenUpdateIsInvalid))]
    [Trait("Application ", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs), 
        parameters: 12,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
        )
    ]
    public void TestWhenUpdateIsInvalid(UseCases.UpdateCategoryInput input, string expectedErrorMessage)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var category = _fixture.GetValidCategory();
        repositoryMock.Setup(x => x.Get(
                input.Id,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(category);
        var useCase = new UseCases.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        task.Should().ThrowAsync<EntityValidationException>().WithMessage(expectedErrorMessage);
        repositoryMock.Verify(x => x.Get(
                input.Id, 
                It.IsAny<CancellationToken>()
                ), 
            Times.Once);
    }
}