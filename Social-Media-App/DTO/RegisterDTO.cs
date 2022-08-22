using System.ComponentModel.DataAnnotations;

namespace Social_Media_App.DTO;

public class RegisterDTO
{
    [Required]    
    public string UserName { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}