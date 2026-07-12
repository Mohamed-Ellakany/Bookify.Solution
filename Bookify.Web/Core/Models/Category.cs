namespace Bookify.Web.Core.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; } = null!;


        public ICollection<BookCategory> CategoryBooks { get; set; } = new HashSet<BookCategory>();


    }
}
