using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LibraryController(ILibraryService service) : ControllerBase
{
    [Route("[action]")]
    [HttpGet]
    public ActionResult<IEnumerable<Book>> Get()
    {
        throw new NotImplementedException();
    }
    
    [Route("[action]")]
    [HttpPost]
    public ActionResult<Book> Post([FromBody] CreateBookDto book)
    {
        var newBook = service.AddBook(book);
        return Ok(newBook);
    }
    
}