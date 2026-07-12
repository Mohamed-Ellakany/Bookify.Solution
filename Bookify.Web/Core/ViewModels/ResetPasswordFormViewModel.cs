namespace Bookify.Web.Core.ViewModels
{
    public class ResetPasswordFormViewModel
    {
        public string? Id { get; set; }

        [StringLength(100, ErrorMessage = Errors.MinMaxError, MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression(RegexPatterns.PasswordPattern, ErrorMessage = Errors.WeakPassword)]
        public string Password { get; set; } = default!;


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = Errors.ConfirmPasswordNotMatch)]
        public string? ConfirmPassword { get; set; }

    }
}
