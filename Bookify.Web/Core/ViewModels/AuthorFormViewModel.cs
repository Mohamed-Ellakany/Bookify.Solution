namespace Bookify.Web.Core.ViewModels
{
    public class AuthorFormViewModel
    {

        public int? Id { get; set; }
        [MaxLength(100, ErrorMessage = Errors.MaxLengthError), MinLength(2, ErrorMessage = Errors.MinLengthError), Required(ErrorMessage = Errors.RequiredError), Display(Name = "Author Name")]
        [RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
        [Remote("AllowItem", "Authors", AdditionalFields = nameof(Id), ErrorMessage = Errors.DublicatedError    )]
        public string Name { get; set; }
    }
}
