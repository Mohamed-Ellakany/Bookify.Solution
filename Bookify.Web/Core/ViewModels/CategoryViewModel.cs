namespace Bookify.Web.Core.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; } = null!;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime? LastUpdatedOn { get; set; }

        public bool IsDeleted { get; set; }
       


    }
}
