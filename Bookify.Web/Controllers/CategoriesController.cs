namespace Bookify.Web.Controllers
{
    [Authorize(Roles = AppRoles.Archive)]
    public class CategoriesController(ApplicationDbContext context, IMapper mapper) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public IActionResult Index()
        {
            var categories = _context.Categories.AsNoTracking().ToList();
            var viewModel = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
            return View(viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView("_Form");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryFormViewModel categoryVM)
        {
            if (!ModelState.IsValid)
                return PartialView("_Form", categoryVM);

            var category = _mapper.Map<Category>(categoryVM);

            category.CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            category.CreatedOn = DateTime.Now;

            _context.Categories.Add(category);
            _context.SaveChanges();

            var categoryVm = _mapper.Map<CategoryViewModel>(category);

            return PartialView("_CategoryRow", categoryVm);
        }
       
        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }
            var categoryVM =_mapper.Map<CategoryFormViewModel>(category);

            return PartialView("_Form", categoryVM);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryFormViewModel categoryVM)
        {
            if (!ModelState.IsValid)
                return PartialView("_Form", categoryVM);

            var category = _context.Categories.Find(categoryVM.Id);
            if (category is null)
            {
                return NotFound();
            }

           category = _mapper.Map(categoryVM, category);
            
            category.LastUpdatedOn = DateTime.Now;
            category.UpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            _context.Categories.Update(category);
            _context.SaveChanges();

            var categoryViewModel = _mapper.Map<CategoryViewModel>(category);


            return PartialView("_CategoryRow", categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id) 
        {
           var category= _context.Categories.Find(id);
            if (category is null) return NotFound();

            category.IsDeleted = !category.IsDeleted;
            category.DeletedOn = null;

            category.LastUpdatedOn = DateTime.Now;
            category.UpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            
            if (!category.IsDeleted)
                category.DeletedOn= DateTime.Now;

            _context.SaveChanges();

            return Ok(category.LastUpdatedOn.ToString());

        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult AllowItem(CategoryFormViewModel model)
        {
            var IsExist = _context.Categories.Any(c => c.Name.ToLower().Trim() == model.Name.ToLower().Trim() && c.Id != model.Id); 
            
            return IsExist ? Json($"Category with name {model.Name} already exists.") : Json(true);

        }
    }

}
