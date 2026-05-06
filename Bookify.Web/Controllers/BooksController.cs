using AutoMapper;
using Bookify.Web.Core.Consts;
using Bookify.Web.Data;
using Bookify.Web.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Bookify.Web.Controllers
{
    public class BooksController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, IOptions<CloudinarySettings> options) : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private int _maxAllowedSize = 2097152;

        private readonly Cloudinary _cloudinary = new Cloudinary(new Account()
        {
            ApiKey = options.Value.ApiKey,
            ApiSecret = options.Value.ApiSecret,
            Cloud = options.Value.Cloud
        });

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View("Form", PopulateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", PopulateViewModel(model));

            var book = _mapper.Map<Book>(model);

            if (model.Image is not null)
            {
                var extension = Path.GetExtension(model.Image.FileName);

                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.NotAllowedExtension);
                    return View("Form", PopulateViewModel(model));
                }

                if (model.Image.Length > _maxAllowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.MaxSize);
                    return View("Form", PopulateViewModel(model));
                }

                using var stream = model.Image.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(model.Image.FileName, stream)
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                book.ImageUrl = result.SecureUrl.ToString();
                book.ImagePublicId = result.PublicId;
                book.ImageThumbnailUrl = GetThmbnailUrl(book.ImageUrl);
            }

            foreach (var category in model.SelectedCategories)
                book.BookCategories.Add(new BookCategory { CategoryId = category });

            _context.Add(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var book = _context.Books.Include(b => b.BookCategories).SingleOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound();

            var model = _mapper.Map<BookFormViewModel>(book);
            var viewModel = PopulateViewModel(model);
            viewModel.SelectedCategories = book.BookCategories.Select(bc => bc.CategoryId).ToList();

            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", PopulateViewModel(model));

            var book = _context.Books.Include(b => b.BookCategories).SingleOrDefault(b => b.Id == model.Id);

            if (book is null)
                return NotFound();

            // Preserve existing image info before mapping overwrites them
            var existingImageUrl = book.ImageUrl;
            var existingImagePublicId = book.ImagePublicId;
            var existingImageThumbnailUrl = book.ImageThumbnailUrl;

            if (model.Image is not null)
            {
                // Delete old image from Cloudinary if it exists
                if (!string.IsNullOrEmpty(existingImagePublicId))
                    await _cloudinary.DeleteResourcesAsync(existingImagePublicId);

                var extension = Path.GetExtension(model.Image.FileName);

                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.NotAllowedExtension);
                    return View("Form", PopulateViewModel(model));
                }

                if (model.Image.Length > _maxAllowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.MaxSize);
                    return View("Form", PopulateViewModel(model));
                }

                using var stream = model.Image.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(model.Image.FileName, stream)
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                if (result.Error is not null)
                {
                    ModelState.AddModelError(nameof(model.Image), result.Error.Message);
                    return View("Form", PopulateViewModel(model));
                }

                // Map all non-image fields from model onto book
                book = _mapper.Map(model, book);

                // Apply new image data
                book.ImageUrl = result.SecureUrl.ToString();
                book.ImagePublicId = result.PublicId;
                book.ImageThumbnailUrl = GetThmbnailUrl(book.ImageUrl);
            }
            else
            {
                // No new image uploaded — map non-image fields, then restore existing image data
                book = _mapper.Map(model, book);

                book.ImageUrl = existingImageUrl;
                book.ImagePublicId = existingImagePublicId;
                book.ImageThumbnailUrl = existingImageThumbnailUrl;
            }

            book.LastUpdatedOn = DateTime.Now;

            book.BookCategories.Clear();
            foreach (var category in model.SelectedCategories)
            {
                book.BookCategories.Add(new BookCategory
                {
                    BookId = book.Id,
                    CategoryId = category
                });
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AllowItem(BookFormViewModel model)
        {
            var book = _context.Books.SingleOrDefault(b => b.Title == model.Title && b.AuthorId == model.AuthorId);
            var isAllowed = book is null || book.Id.Equals(model.Id);

            return Json(isAllowed);
        }

        private BookFormViewModel PopulateViewModel(BookFormViewModel? viewModel = null)
        {
            viewModel = viewModel is null ? new BookFormViewModel() : viewModel;

            var categories = _context.Categories.Where(c => !c.IsDeleted).OrderBy(c => c.Name).ToList();
            var authors = _context.Authors.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();

            viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories);
            viewModel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);

            return viewModel;
        }

        private string GetThmbnailUrl(string ImageUrl)
        {
            var parts = ImageUrl.Split("image/upload/");
            var thumbnailUrl = $"{parts[0]}image/upload/c_thumb,w_200,g_face/{parts[1]}";
            return thumbnailUrl;
        }
    }
}