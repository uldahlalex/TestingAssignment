using Generated;

namespace Api.IntegrationTests.Tests;

public class BooksTests : ApiTestBase
{
    [Fact]
    public async Task CreateBook_CanSuccessFullyCreateBook()
    {
        var dto = new CreateBookDto()
        {
            Author = "A",
            Genre = "A",
            Title = "A"
        };
        var result = (await new LibraryClient(Client).PostAsync(dto)).Result;
        Assert.Equivalent(result.Author, dto.Author);
        Assert.Equivalent(result.Genre, dto.Genre);
        Assert.Equivalent(result.Title, dto.Title);
        Assert.NotEqual(0, result.Id);
    }
}