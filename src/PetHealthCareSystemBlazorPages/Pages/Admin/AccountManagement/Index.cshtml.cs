using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.DTO.User;
using Service.IServices;
using Microsoft.AspNetCore.Mvc;
using Repository.Extensions;
using Utility.Enum;

namespace PetHealthCareSystemRazorPages.Pages.Admin.AccountManagement
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        public PaginatedList<UserResponseDto> Users { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? pageNumber)
        {
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            var role = HttpContext.Session.GetString("Role");

            if (role == null || !role.Contains(UserRole.Admin.ToString()))
            {
                return RedirectToPage("/Login");
            }

            Users = await _userService.GetAllUsersAsync(pageNumber ?? 1, PageSize);
            return Page();
        }
    }
}
