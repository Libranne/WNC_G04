using Microsoft.AspNetCore.Mvc;
using WNC_G04.Models;
using System.IO;

namespace WNC_G04.Controllers
{
    public class TrangCaNhanController : Controller
    {
        private readonly DbG04RVContext _context;

        public TrangCaNhanController(DbG04RVContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TrangCaNhan()
        {
            var currentUserEmail = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(currentUserEmail))
            {
                return RedirectToAction("Login", "Auth"); // Điều hướng nếu chưa đăng nhập
            }

            var nguoiDung = _context.NguoiDungs.SingleOrDefault(nd => nd.Email == currentUserEmail);
            if (nguoiDung == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            // Lấy danh sách bài viết, người theo dõi, người được theo dõi
            var baiViets_nd = _context.BaiViets
                .Where(b => b.MaNguoiDung == nguoiDung.MaNguoiDung)
                .Select(b => new BaiViet
                {
                    MaBaiViet = b.MaBaiViet,
                    MaNguoiDung = b.MaNguoiDung,
                    TenNguoiDung = b.MaNguoiDungNavigation.TenNguoiDung,
                    AnhDaiDien = b.MaNguoiDungNavigation.AnhDaiDien,
                    TenChuDe = b.MaChuDeNavigation.TenChuDe,
                    NoiDung = b.NoiDung,
                    AnhBaiViet = b.AnhBaiViet,
                    NgayTao = b.NgayTao ?? DateTime.Now,
                    Thiches = b.Thiches,
                    IsLiked = _context.Thiches.Any(t => t.MaBaiViet == b.MaBaiViet && t.MaNguoiDung == nguoiDung.MaNguoiDung),
                    SoLuongLike = b.SoLuongLike
                }).ToList();

            var dctheodoi_nd = _context.TheoDois
                .Where(td => td.MaNguoiTheoDoi == nguoiDung.MaNguoiDung)
                .ToList();

            var theodoi_nd = _context.TheoDois
                .Where(td => td.MaNguoiDuocTheoDoi == nguoiDung.MaNguoiDung)
                .ToList();

            // Gán dữ liệu vào ViewBag
            ViewBag.nguoiD = nguoiDung;
            ViewBag.baiV = baiViets_nd;
            ViewBag.SoNguoi_DcTheoDoi = dctheodoi_nd.Count;
            ViewBag.SoNguoi_TheoDoi = theodoi_nd.Count;

            return View();
        }

        [HttpPost]
        [Route("TrangCaNhan/editProfile")]
        public async Task<IActionResult> EditProfile([FromForm] IFormFile image = null, [FromForm] string tenND = "", [FromForm] string TieuSu = "")
        {
            var currentUserEmail = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(currentUserEmail))
            {
                return RedirectToAction("Login", "Auth"); // Điều hướng nếu chưa đăng nhập
            }

            var nguoiDung = _context.NguoiDungs.SingleOrDefault(nd => nd.Email == currentUserEmail);
            if (nguoiDung == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            if (image != null)
            {
                // Định nghĩa đường dẫn lưu ảnh
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "User", "img");

                // Kiểm tra nếu thư mục chưa tồn tại thì tạo thư mục
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var uploadPath = Path.Combine(directoryPath, image.FileName);

                // Kiểm tra nếu tệp đã tồn tại
                if (!System.IO.File.Exists(uploadPath))
                {
                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                }

                nguoiDung.AnhDaiDien = $"/User/img/{image.FileName}";
            }

            if (!string.IsNullOrWhiteSpace(tenND))
            {
                nguoiDung.TenNguoiDung = tenND;
            }

            if (!string.IsNullOrWhiteSpace(TieuSu))
            {
                nguoiDung.TieuSu = TieuSu;
            }

            try
            {
                _context.Update(nguoiDung);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Không thể cập nhật thông tin: " + ex.Message);
            }

            return RedirectToAction("TrangCaNhan");
        }

    }
}
