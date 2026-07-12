namespace Bookify.Web.Core.Models
{
    [Index(nameof(Name) , IsUnique = true)]
    public class Author : BaseModel
    {
        public int Id { get; set; }
        [MaxLength(100 , ErrorMessage ="Max length is 100 character") , MinLength(2 ,ErrorMessage ="Min length is 2 character"),Required(ErrorMessage ="Name Field is required")]
        public string Name { get; set; }
    }
}
