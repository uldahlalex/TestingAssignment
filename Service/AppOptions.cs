using System.ComponentModel.DataAnnotations;

namespace service.Types;

public class AppOptions
{
    [Required] [MinLength(1)] public string Database { get; set; } = null!;
    [Required] public bool RunInTestContainer { get; set; } = false;
}