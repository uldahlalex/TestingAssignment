using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LibraryController(ILibraryService service) : ControllerBase
{
    [Route("[action]")]
    [Authorize]
    [HttpGet]
    public ActionResult<IEnumerable<Book>> Get()
    {
        throw new NotImplementedException();
    }

    [Route("[action]")]
    [Authorize]
    [HttpPost]
    public ActionResult<Book> Post([FromBody] CreateBookDto book)
    {
        var newBook = service.AddBook(book);
        return Ok(newBook);
    }
}