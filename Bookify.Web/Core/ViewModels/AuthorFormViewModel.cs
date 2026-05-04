using Bookify.Web.Core.Consts;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Bookify.Web.Core.ViewModels
{
    public class AuthorFormViewModel
    {

        public int? Id { get; set; }
        [MaxLength(100, ErrorMessage = Errors.MaxLengthError), MinLength(2, ErrorMessage = Errors.MinLengthError), Required(ErrorMessage = Errors.RequiredError), Display(Name = "Author Name")]
        [Remote("AllowItem", "Authors", AdditionalFields = nameof(Id), ErrorMessage = Errors.DublicatedError    )]
        public string Name { get; set; }
    }
}
