namespace Bookify.Web.Core.ViewModels
{
    public class BookViewModel
    {

        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!; 

        public string Publisher { get; set; } = null!;

        public DateTime PublishingDate { get; set; }

        public string? ImageUrl { get; set; }
        public string? ImageThumbnailUrl { get; set; }


        public string Hall { get; set; } = null!;

        public bool IsAvailableForRental { get; set; }

        public string Description { get; set; } = null!;

        public IEnumerable<string> BookCategories { get; set; } = null!;
        public IEnumerable<BookCopyViewModel> BookCopies { get; set; } = null!;

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string? CreatedById { get; set; }

        public DateTime? LastUpdatedOn { get; set; }
        public string? UpdatedById { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedById { get; set; }
    }
}
