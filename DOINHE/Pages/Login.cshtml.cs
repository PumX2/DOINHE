using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DOINHE.Entitys;
using System.Text.Json;
using DOINHE.Db;
using System.ComponentModel.DataAnnotations;

namespace DOINHE.Pages
{
    public class LoginModel : PageModel
    {
        private readonly MyDbContext _dbContext;

        public LoginModel(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public User User { get; set; }

        public IActionResult OnGet()
        {
            User = new User(); // Ensure model is initialized

            if (HttpContext.Session.GetString("admin") != null || HttpContext.Session.GetString("customer") != null)
            {
                return RedirectToPage("");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    // Optional: Log or display the errors
            //    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            //    TempData["msg"] = string.Join(", ", errors); // Show validation errors
            //    return Page();
            //}

            // Tìm user với email khớp
            var member = await _dbContext.Users.SingleOrDefaultAsync(a => a.Email.Equals(User.Email));

            if (member != null)
            {
                // So sánh mật khẩu người dùng đã nhập và mật khẩu trong cơ sở dữ liệu
                if (member.Password == User.Password)
                {
                    // Đăng nhập thành công
                    HttpContext.Session.SetString("Account", JsonSerializer.Serialize(member));

                    if (member.Role == 1)
                    {
                        HttpContext.Session.SetString("admin", JsonSerializer.Serialize(member));
                        return RedirectToPage("./Admin/Orders/Index");
                    }
                    if (member.Role == 2)
                    {
                        HttpContext.Session.SetString("customer", JsonSerializer.Serialize(member));
                        HttpContext.Session.SetString("name", member.Name);
                        return RedirectToPage("/Index");
                    }
                }
                else
                {
                    // Mật khẩu không chính xác
                    TempData["msg"] = "Invalid password.";
                    return Page();
                }
            }

            // Không tìm thấy user với email đã nhập
            TempData["msg"] = "Email or password invalid.";
            return Page();
        }
    }
}
