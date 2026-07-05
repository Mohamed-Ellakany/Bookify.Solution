namespace Bookify.Web.Core.ViewModels
{
    public class BookCopyViewModel
    {
        public int Id { get; set; }
        public int SerialNumber { get; set; }
        public int EditionNumber { get; set; }
        public bool IsAvailableForRental { get; set; }
        public string? BookTitle { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
    }
}
