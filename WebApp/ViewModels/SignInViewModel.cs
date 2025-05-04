using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;
public class SignInViewModel
{
    [Required]
    [Display(Name = "Email", Prompt = "Enter email adress")]
    [RegularExpression(@"")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "Password", Prompt = "Enter password")]
    [RegularExpression(@"")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public bool IsPersistent { get; set; }

}
