using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using WNC_G04.Models;
using WNC_G04.Session;
using System.Linq;
using System.Threading.Tasks;

namespace WNC_G04.Controllers
{
    public class AccessController : Controller
    {
        private readonly DbG04RVContext _context;
        private readonly int MaxLoginAttempts = 3;
        private readonly int LockoutDurationInMinutes = 30;

        //Tạo biên để xử lý lưu người dùng vào Session
        private ssNguoiDung ssNguoiDung;

        public AccessController(DbG04RVContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult LogIn(NguoiDung nguoiDung)
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                // Lấy số lần thử đăng nhập từ session
                int loginAttempts = HttpContext.Session.GetInt32("LoginAttempts") ?? 0;

                // Lấy thời gian khóa tài khoản từ session, nếu có
                string lockoutEndString = HttpContext.Session.GetString("LockoutEnd");
                DateTime? lockoutEnd = null;

                if (!string.IsNullOrEmpty(lockoutEndString))
                {
                    lockoutEnd = DateTime.Parse(lockoutEndString);
                }

                // Kiểm tra nếu tài khoản đang bị khóa
                if (lockoutEnd.HasValue && lockoutEnd > DateTime.Now)
                {
                    TimeSpan remainingLockout = lockoutEnd.Value - DateTime.Now;
                    ModelState.AddModelError("", $"Bạn đã nhập sai mật khẩu 3 lần. Vui lòng thử lại sau {remainingLockout.Minutes} phút {remainingLockout.Seconds} giây.");
                    return View();
                }

                // Kiểm tra thông tin đăng nhập
                var user = _context.NguoiDungs.FirstOrDefault(x => x.Email.Equals(nguoiDung.Email));

                if (user != null)
                {
                    // Kiểm tra mật khẩu
                    if (user.MatKhau.Equals(nguoiDung.MatKhau))
                    {
                        // Đăng nhập thành công
                        HttpContext.Session.SetString("Email", user.Email);
                        HttpContext.Session.SetInt32("id", user.MaNguoiDung);
                        HttpContext.Session.SetString("tenND", user.TenNguoiDung);

                        // Reset số lần đăng nhập sai
                        HttpContext.Session.SetInt32("LoginAttempts", 0);
                        HttpContext.Session.Remove("LockoutEnd");

                        return RedirectToAction("ListBaiViet", "Trangchu");
                    }
                    else
                    {
                        // Tăng số lần thử đăng nhập
                        loginAttempts++;
                        HttpContext.Session.SetInt32("LoginAttempts", loginAttempts);

                        if (loginAttempts >= 3)
                        {
                            // Khóa tài khoản trong 30 phút
                            lockoutEnd = DateTime.Now.AddMinutes(30);
                            HttpContext.Session.SetString("LockoutEnd", lockoutEnd.ToString());  // Lưu dưới dạng chuỗi

                            // Đặt lại số lần thử đăng nhập
                            HttpContext.Session.SetInt32("LoginAttempts", 0);

                            ModelState.AddModelError("", "Bạn đã nhập sai mật khẩu 3 lần. Vui lòng thử lại sau 30 phút.");
                            return View();
                        }
                        else
                        {
                            int remainingAttempts = 3 - loginAttempts;
                            ModelState.AddModelError("", $"Tên đăng nhập hoặc mật khẩu không đúng. Bạn còn {remainingAttempts} lần thử.");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return View(nguoiDung);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(NguoiDung model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem email hoặc tên người dùng đã tồn tại chưa
                var existingUserByEmail = _context.NguoiDungs
                    .FirstOrDefault(u => u.Email == model.Email);
                var existingUserByUserName = _context.NguoiDungs
                    .FirstOrDefault(u => u.TenNguoiDung == model.TenNguoiDung);

                // Thông báo lỗi nếu email hoặc tên người dùng đã tồn tại
                if (existingUserByEmail != null)
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                }

                if (existingUserByUserName != null)
                {
                    ModelState.AddModelError("TenNguoiDung", "Tên người dùng đã được sử dụng.");
                }

                // Nếu có lỗi trong ModelState, trả về lại View để hiển thị lỗi
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // Kiểm tra mật khẩu và xác nhận mật khẩu có trùng khớp không
                if (model.MatKhau != model.ConfirmMatKhau)
                {
                    ModelState.AddModelError("ConfirmMatKhau", "Mật khẩu và xác nhận mật khẩu không khớp.");
                    return View(model);
                }

                var newUser = new NguoiDung
                {
                    TenNguoiDung = model.TenNguoiDung,
                    Email = model.Email,
                    MatKhau = model.MatKhau,  // Mã hóa mật khẩu nếu cần
                    ConfirmMatKhau = model.ConfirmMatKhau,
                    NgayTao = DateTime.Now,
                    FailedLoginAttempts = 0  // Khởi tạo số lần đăng nhập sai
                };

                try
                {
                    _context.NguoiDungs.Add(newUser);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("LogIn", "Access");
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException?.Message;
                    ModelState.AddModelError("", $"Đã xảy ra lỗi khi lưu thông tin người dùng. Chi tiết: {innerException}");
                }
            }

            // Kiểm tra và hiển thị các lỗi ModelState nếu có
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                Console.WriteLine($"Errors: {errorMessages}");
            }

            return View(model);
        }

        // public IActionResult LogOut()
        // {
        //     HttpContext.Session.Clear();
        //     HttpContext.Session.Remove("Username");
        //     return RedirectToAction("LogIn", "Access");
        // }
    }
}
