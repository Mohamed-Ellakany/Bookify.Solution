using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace Bookify.Web.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class UsersController(UserManager<ApplicationUser> userManager, IMapper mapper,RoleManager<IdentityRole> roleManager , IEmailSender emailSender , IEmailBodyBuilder emailBodyBuilder) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IEmailBodyBuilder _emailBodyBuilder = emailBodyBuilder;
        public async Task<IActionResult> Index()
        {
           
            var users = await _userManager.Users.ToListAsync();
            var viewModels = _mapper.Map<IEnumerable<UserViewModel>>(users);

            return View(viewModels);

        }


        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> Create()
        {
            var viewModel = new UserFormViewModel
            {
                Roles = await _roleManager.Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToListAsync()
            };

            return PartialView("_Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ApplicationUser user = new()
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                FullName = viewModel.FullName,
                CreatedOn = DateTime.UtcNow,
                CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
            };

            var result = await _userManager.CreateAsync(user, viewModel.Password!);

            if (result.Succeeded)
            {

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code},
                    protocol: Request.Scheme)!;


             
                var body = _emailBodyBuilder.GetEmailBody("https://res.cloudinary.com/ellakanydev/image/upload/v1783873900/icon-positive-vote-1_rdexez_a909y0.svg",
                     $"Hey {user.FullName} , thanks for joining us.",
                     "Please Confirm Your Email ",
                      HtmlEncoder.Default.Encode(callbackUrl),
                      "Active Account"
                     );

                await _emailSender.SendEmailAsync(user.Email, "Confirm your email", body);





                await _userManager.AddToRolesAsync(user, viewModel.SelectedRoles);
                var viewModelToReturn = _mapper.Map<UserViewModel>(user);

                return PartialView("_UserRow", viewModelToReturn);

            }

            return BadRequest(string.Join("," , result.Errors.Select(e=>e.Description)));
        }

        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> ResetPassword(string Id) 
        {
          var user =await  _userManager.FindByIdAsync(Id);
            if(user is null)
                return NotFound();

            return PartialView("_ResetPasswordForm", new ResetPasswordFormViewModel { Id = user.Id });

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordFormViewModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest();  

            var user = await _userManager.FindByIdAsync(model.Id!);

            if (user is null)
                return NotFound();

            var currentPassword = user.PasswordHash;
            
            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, model.Password);

            if (result.Succeeded)
            {
                user.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                user.LastUpdatedOn = DateTime.Now;

                await _userManager.UpdateAsync(user);

                var viewModel = _mapper.Map<UserViewModel>(user);

                return PartialView("_UserRow" , viewModel);

            }

            user.PasswordHash = currentPassword;
            await _userManager.UpdateAsync(user);

            return BadRequest(string.Join(",", result.Errors.Select(e => e.Description)));

        }

        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> Edit(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user is null)
                return NotFound();
            
            var viewModel = _mapper.Map<UserFormViewModel>(user);
            
            viewModel.Roles = await _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToListAsync();

            viewModel.SelectedRoles = await _userManager.GetRolesAsync(user);
            
            return PartialView("_Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserFormViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(model.Id!);
            
            if (user is null)
                return NotFound();
         
            user =  _mapper.Map(model, user);

            user.LastUpdatedOn = DateTime.Now;
            user.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _userManager.UpdateAsync(user);


            if (result.Succeeded)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                
                var rolesUpdated = !currentRoles.SequenceEqual(model.SelectedRoles);
                if (rolesUpdated) 
                {
                    await _userManager.RemoveFromRolesAsync(user , currentRoles);
                    await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                }

                await _userManager.UpdateSecurityStampAsync(user);


                var viewModel = _mapper.Map<UserViewModel>(user);
                return PartialView("_UserRow", viewModel);

            }
            return BadRequest(string.Join(",", result.Errors.Select(e => e.Description)));
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var user =await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            user.IsDeleted = !user.IsDeleted;
            user.LastUpdatedOn = DateTime.Now;
            user.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            await _userManager.UpdateAsync(user);
            
            if(user.IsDeleted)
                await _userManager.UpdateSecurityStampAsync(user);

            return Ok(user.LastUpdatedOn.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

           var islocked = await _userManager.IsLockedOutAsync(user);

            if (islocked)
            {

            await _userManager.SetLockoutEndDateAsync(user,null);
            }

            return Ok();

        }



        public async Task<IActionResult> AllowEmail(UserFormViewModel viewModel)
        {
            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            var isAllowed = user is null || user.Id.Equals(viewModel.Id);

            return Json(isAllowed);
        }
        public async Task<IActionResult> AllowUserName(UserFormViewModel viewModel)
        {
            var user = await _userManager.FindByNameAsync(viewModel.UserName);
            var isAllowed = user is null || user.Id.Equals(viewModel.Id);

            return Json(isAllowed);
        }
    }
}
