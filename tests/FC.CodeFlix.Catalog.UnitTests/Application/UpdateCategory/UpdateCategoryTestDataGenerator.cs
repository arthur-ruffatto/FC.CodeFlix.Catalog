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
            var input = fixture.GetValidInput(
                category.Id
            );
            yield return
            [
                category, 
                input
            ];
        }
    }
}