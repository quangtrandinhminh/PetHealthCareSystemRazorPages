using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.DTO.User;
using Service.IServices;
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

        public IList<UserResponseDto> Users { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Users = await _userService.GetAllUsersByRoleAsync(UserRole.Vet);
        }
    }
}
