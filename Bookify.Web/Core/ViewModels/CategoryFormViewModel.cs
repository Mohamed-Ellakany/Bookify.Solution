namespace Bookify.Web.Core.ViewModels
{
    public class CategoryFormViewModel
    {
        public int? Id { get; set; }
        
        [MaxLength(100 ,ErrorMessage =Errors.MaxLengthError) , MinLength(2 , ErrorMessage = Errors.MinLengthError),Required(ErrorMessage =Errors.RequiredError),Display(Name = "Category Name")]
        [RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
        [Remote("AllowItem", "Categories" , AdditionalFields = nameof(Id) , ErrorMessage = Errors.DublicatedError)]
        public string Name { get; set; }

    }
}
