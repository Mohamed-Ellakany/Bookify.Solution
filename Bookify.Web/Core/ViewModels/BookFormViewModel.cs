using Bookify.Web.Core.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Bookify.Web.Core.ViewModels
{
    public class BookFormViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = Errors.RequiredError), MaxLength(500 , ErrorMessage = Errors.MaxLengthError)]
        [Remote("AllowItem", null!, AdditionalFields = "Id,AuthorId", ErrorMessage = Errors.DublicatedBookWithTheSameAuthor)]
        public string Title { get; set; } = null!;

        [Display(Name = "Author")]
        [Remote("AllowItem"  , null! ,AdditionalFields = "Id,Title" , ErrorMessage = Errors.DublicatedAuthorWithTheSameBook)]
        public int AuthorId { get; set; }
        public IEnumerable<SelectListItem>? Authors { get; set; }

        [Required(ErrorMessage = Errors.RequiredError), MaxLength(200 , ErrorMessage = Errors.MaxLengthError)]
        public string Publisher { get; set; } = null!;
        [Display(Name = "Publishing Date")]
        [AssertThat("PublishingDate <= Today()", ErrorMessage = Errors.InvalidPublishingDate)]
        public DateTime PublishingDate { get; set; } = DateTime.Now;
        public string? ImageUrl { get; set; } = null!;
        public IFormFile? Image { get; set; }

        [MaxLength(50 , ErrorMessage = Errors.MaxLengthError)]
        public string Hall { get; set; } = null!;

        [Display(Name = "Is Available for Rental?")]
        public bool IsAvailableToRental { get; set; }
        public string Description { get; set; } = null!;
        [Display(Name = "Categories")]
        public IList<int> SelectedCategories { get; set; } = new List<int>();
        public IEnumerable<SelectListItem>? Categories { get; set; }

    }
}
