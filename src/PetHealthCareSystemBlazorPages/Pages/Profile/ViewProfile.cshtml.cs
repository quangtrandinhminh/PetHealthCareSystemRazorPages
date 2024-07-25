using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using BusinessObject.DTO.User;

namespace PetHealthCareSystemRazorPages.Pages.Profile
{
    public class ViewProfileModel : PageModel
    {
        private readonly IUserService _userService;
        public ViewProfileModel(IUserService userService)
        {
            _userService = userService;
        }

        public UserResponseDto UserResponse { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Lấy UserId từ session
                string userIdString = HttpContext.Session.GetString("UserId");

                // Kiểm tra nếu UserId không tồn tại trong session
                if (string.IsNullOrEmpty(userIdString))
                {
                    return RedirectToPage("/Index");
                }

                // Chuyển đổi UserId thành int
                if (!int.TryParse(userIdString, out int userId))
                {
                    return RedirectToPage("/Index");
                }

                // Lấy thông tin người dùng từ UserService
                UserResponse = await _userService.GetByIdAsync(userId);

                // Kiểm tra nếu không tìm thấy người dùng
                if (UserResponse == null)
                {
                    return RedirectToPage("/Index");
                }
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
