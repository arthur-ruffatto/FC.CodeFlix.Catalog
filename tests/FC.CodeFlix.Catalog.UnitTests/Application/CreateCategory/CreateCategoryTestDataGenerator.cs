namespace FC.CodeFlix.Catalog.UnitTests.Application.CreateCategory;

public class CreateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int numberOfTests)
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputList = new List<object[]>();
        var totalInvalidCases = 4;

        for (int i = 0; i < numberOfTests; i++)
        {
            switch (i % totalInvalidCases)
            {
                //When name is less than 3 characters
                case 0:
                    invalidInputList.Add(new object[]
                    {
                        fixture.GetNameTooShort(), 
                        "Name should be at least 3 characters long"
                    });
                    break;
                
                //When name is longer than 255 characters
                case 1:
                    invalidInputList.Add(new object[]
                    {
                        fixture.GetNameTooLong(), 
                        "Name should not be longer than 255 characters"
                    });
                    break;
                
                //When description is null
                case 2:
                    invalidInputList.Add(new object[]
                    {
                        fixture.GetNullDescription(), 
                        "Description should not be null"
                    });
                    break;
                
                //When description is longer than 10_000 characters
                case 3:
                    invalidInputList.Add(new object[]
                    {
                        fixture.GetDescriptionTooLong(), 
                        "Description should not be longer than 10000 characters"
                    });
                    break;
                default:
                    break;
            }
        }
        
        return invalidInputList;
    }
}