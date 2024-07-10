using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.DTO.User;
using Service.IServices;
using Utility.Constants;
using Utility.Exceptions;

namespace PetHealthCareSystemRazorPages.Pages.Admin.AccountManagement
{
    public class DetailsModel : PageModel
    {
        private readonly IUserService _userService;

        public DetailsModel(IUserService userService)
        {
            _userService = userService;
        }

        public UserResponseDto User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                User = await _userService.GetVetByIdAsync(id.Value);
            }
            catch (AppException ex) when (ex.Code == ResponseCodeConstants.NOT_FOUND)
            {
                return NotFound(ex.Message);
            }

            return Page();
        }
    }
}
