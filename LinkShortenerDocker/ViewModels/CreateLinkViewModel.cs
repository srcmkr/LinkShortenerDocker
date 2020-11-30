using System.ComponentModel.DataAnnotations;

namespace LinkShortenerDocker.ViewModels
{
    public class CreateLinkViewModel
    {
        [Required] public string NewLink { get; set; }
        [Required] public string Password { get; set; }
    }
}
