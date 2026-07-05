using AutoMapper;
using Bookify.Web.Data;
using Bookify.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Web.Controllers
{
    public class BookCopiesController(ApplicationDbContext context , IMapper mapper) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        [AjaxOnly]
        public IActionResult Create(int bookId)
        {
            var book = _context.Books.Find(bookId);
            if(book is null)
                return NotFound();

            var viewModel = new BookCopyFormViewModel { BookId = bookId , ShowRentalInput = book.IsAvailableForRental };
            
            return PartialView("Form" , viewModel);
        }

        [HttpPost]
        public IActionResult Create(BookCopyFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
                 return BadRequest();
            
            var book = _context.Books.Find(viewModel.BookId);
            if(book is null)
                return NotFound();

            BookCopy copy = new()
            {
                EditionNumber = viewModel.EditionNumber,
                IsAvailableForRental = book.IsAvailableForRental && viewModel.IsAvailableForRental,
            };

            book.BookCopies.Add(copy);

            _context.SaveChanges();

            var vm= _mapper.Map<BookCopyViewModel>(copy);

            return PartialView("_BookCopyRow" , vm);
        }

        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var copy = _context.BookCopies.Include(c=>c.Book).SingleOrDefault(c=>c.Id == id);

            if (copy is null)
                return NotFound();

            var viewModel = _mapper.Map<BookCopyFormViewModel>(copy);
            viewModel.ShowRentalInput = copy.Book!.IsAvailableForRental;

            return PartialView("Form", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookCopyFormViewModel model) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }

            var copy = _context.BookCopies.Include(c => c.Book).SingleOrDefault(c => c.Id == model.Id);
            if (copy is null)
                return NotFound();

            copy.EditionNumber = model.EditionNumber;
            copy.IsAvailableForRental = copy.Book!.IsAvailableForRental && model.IsAvailableForRental;
            copy.LastUpdatedOn = DateTime.Now;

            _context.SaveChanges();

            var viewModel = _mapper.Map<BookCopyViewModel>(copy);

            return PartialView("_BookCopyRow", viewModel);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var copy = _context.BookCopies.Find(id);

            if (copy is null)
                return NotFound();

            copy.IsDeleted = !copy.IsDeleted;
            copy.LastUpdatedOn = DateTime.Now;

            _context.SaveChanges();

            return Ok();
        }
    }
}
