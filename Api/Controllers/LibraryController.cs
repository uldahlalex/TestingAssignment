using Microsoft.AspNetCore.Mvc;
using Service;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LibraryController(ILibraryService service) : ControllerBase
{
    
    
}