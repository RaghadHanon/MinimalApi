using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Models;
public class AuthenticationRequestBodyDto
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}
