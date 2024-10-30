using Generated;

namespace Api.IntegrationTests.Tests;

public class BooksTests : ApiTestBase
{
    [Fact]
    public async Task CreateBook_CanSuccessFullyCreateBook()
    {
        var dto = new CreateBookDto()
        {
            Author = "Bob",
            
        };
        var result = (await new LibraryClient(Client).PostAsync(dto)).Result;
        Assert.Equivalent(result.Author, dto.Author);
    }
}