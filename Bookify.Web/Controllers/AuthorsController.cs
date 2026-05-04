using AutoMapper;
using Bookify.Web.Data;
using Bookify.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Web.Controllers
{
    public class AuthorsController(ApplicationDbContext context , IMapper mapper) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
       
        public IActionResult Index()
        {
            var Authors =_context.Authors.AsNoTracking().ToList();
            var AuthorsVM = _mapper.Map<IEnumerable<AuthorViewModel>>(Authors);

            return View(AuthorsVM);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView("_Form");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AuthorFormViewModel authorVM)
        {
            if (!ModelState.IsValid)
                return PartialView("_Form", authorVM);

            var author = _mapper.Map<Author>(authorVM);

            _context.Authors.Add(author);
            _context.SaveChanges();

            var authorVm = _mapper.Map<AuthorViewModel>(author);

            return PartialView("_AuthorRow", authorVm);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var author = _context.Authors.Find(id);
            if (author is null)
            {
                return NotFound();
            }
            var authorVM = _mapper.Map<AuthorFormViewModel>(author);

            return PartialView("_Form", authorVM);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorFormViewModel authorVM)
        {
            if (!ModelState.IsValid)
                return PartialView("_Form", authorVM);

            var author = _context.Authors.Find(authorVM.Id);
            if (author is null)
            {
                return NotFound();
            }

            author = _mapper.Map(authorVM, author);
            author.LastUpdatedOn = DateTime.Now;
            _context.Authors.Update(author);
            _context.SaveChanges();

            var authorViewModel = _mapper.Map<AuthorViewModel>(author); 

            return PartialView("_AuthorRow", authorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var author = _context.Authors.Find(id);
            if (author is null) return NotFound();

            author.IsDeleted = !author.IsDeleted;
            author.LastUpdatedOn = DateTime.Now;
            author.DeletedOn = null;

            if (!author.IsDeleted)
                author.DeletedOn = DateTime.Now;

            _context.SaveChanges();

            return Ok(author.LastUpdatedOn.ToString());
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult AllowItem(AuthorFormViewModel model)
        {
            var IsExist = _context.Authors.Any(a => a.Name.ToLower().Trim() == model.Name.ToLower().Trim() && a.Id != model.Id);

            return IsExist ? Json($"Author with name {model.Name} already exists.") : Json(true);
        }


    }
}
