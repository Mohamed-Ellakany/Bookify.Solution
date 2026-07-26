namespace Bookify.Web.Core.ViewModels
{
    public class UserFormViewModel
    {

        public string? Id { get; set; } = null!;

        [MaxLength(200, ErrorMessage = Errors.MaxLengthError) , Display(Name = "Full Name")]
        [RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
        public string FullName { get; set; } = null!;

        [MaxLength(20 , ErrorMessage = Errors.MaxLengthError)]
        [Remote("AllowUserName", null!, AdditionalFields = nameof(Id), ErrorMessage = Errors.DublicatedError)]
        [RegularExpression(RegexPatterns.UserNamePattern , ErrorMessage = Errors.InvalidUserName)]
        public string UserName { get; set; } = null!;

        [MaxLength(100 , ErrorMessage = Errors.MaxLengthError) , EmailAddress]
        [Remote("AllowEmail" , null! , AdditionalFields = nameof(Id) , ErrorMessage = Errors.DublicatedError )]
        public string Email { get; set; } = null!;


        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = Errors.MinMaxError, MinimumLength = 8)]
        [RegularExpression(RegexPatterns.PasswordPattern , ErrorMessage = Errors.WeakPassword)]
        [RequiredIf("Id == null", ErrorMessage = Errors.RequiredError)]
        public string? Password { get; set; } = default!;

      
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = Errors.ConfirmPasswordNotMatch)]
        [RequiredIf("Id == null" , ErrorMessage =Errors.RequiredError)]
        public string? ConfirmPassword { get; set; }


        [Display(Name = "Roles")]
        public IList<string> SelectedRoles { get; set; } = new List<string>();

        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}
