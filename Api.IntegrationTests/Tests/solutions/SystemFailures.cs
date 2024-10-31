using DataAccess;
using Generated;
using Microsoft.Extensions.DependencyInjection;

namespace Api.IntegrationTests.Tests.solutions;

public class SystemFailures : ApiTestBase
{

    [Theory]
    [InlineData("", "asdsad", "asdsad")] // + edge cases
    [InlineData("asd", "", "sad")]
    [InlineData("", "", "")]
    [InlineData(null, "", "")]
    [InlineData("asd", "asd", "")]
    public async Task CreateBook_ShouldFail_DueToDataValidation(string genre, string title, string author)
    {
        var dto = new CreateBookDto()
        {
            Author = author,
            Genre = genre,
            Title = title
        };

        await Assert.ThrowsAnyAsync<ApiException>(async () => await new LibraryClient(Client).PostAsync(dto));
    }
    
    [Fact]
    public async Task CreateBook_DoesntAcceptTwoCharacterGenre_TryCatch()
    {
        var dto = new CreateBookDto()
        {
            Author = "AA",
            Genre = "AA",
            Title = "AA"
        };
        try
        {
            var result = (await new LibraryClient(Client).PostAsync(dto)).Result;
        }
        catch (ApiException e)
        {
            Assert.True(true);
        }
        Assert.Fail();
    }

    [Fact]
    public async Task CreateBook_DoesntAcceptTwoCharacterGenre_ThrowsAsync()
    {
        var dto = new CreateBookDto()
        {
            Author = "AA",
            Genre = "AA",
            Title = "AA"
        };

        await Assert.ThrowsAnyAsync<Exception>(async () => await new LibraryClient(Client).PostAsync(dto));
    }

    [Fact]
    public void MyTest()
    {
        
    }

    
    [Fact]
    public async Task LoanBook_Fails_When_No_Loans_Exist()
    {
        var ctx = ApplicationServices.GetRequiredService<LibraryContext>();
        ctx.Loans.Remove(ctx.Loans.First());
        ctx.SaveChanges();
        
        var dto = new LoanBookDto()
        {
            BookId = 1,
            UserId = 1
        };

        await Assert.ThrowsAnyAsync<Exception>(async () => await new LibraryClient(Client).LoanAsync(dto));
    }
    
    [Theory]
    [InlineData(1,1)]
    public async Task LoanBook_Incorrectly_Can_Loan_Non_Returned_Book(int bookId, int userId)
    {
        var ctx = ApplicationServices.GetRequiredService<LibraryContext>();
        var existing = ctx.Loans.First();
        existing.IsReturned = false;
        ctx.SaveChanges();
        
        var dto = new LoanBookDto()
        {
            BookId = ctx.Books.Find().Id,
            UserId = userId
        };

        await new LibraryClient(Client).LoanAsync(dto);
    }

}