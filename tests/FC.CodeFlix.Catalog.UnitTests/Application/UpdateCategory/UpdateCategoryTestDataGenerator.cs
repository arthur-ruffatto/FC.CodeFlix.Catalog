using FC.CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UpdateCategory;

public class UpdateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int numberOfTests)
    {
        var fixture = new UpdateCategoryTestFixture();

        for (var i = 0; i < numberOfTests; i++)
        {
            var category = fixture.GetValidCategory();
            var input = new UpdateCategoryInput(
                category.Id,
                fixture.GetValidCategoryName(),
                fixture.GetValidCategoryDescription(),
                fixture.GetRandomBoolean()
            );
            yield return
            [
                category, 
                input
            ];
        }
    }
}