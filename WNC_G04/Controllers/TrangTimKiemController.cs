using Microsoft.AspNetCore.Mvc;
using WNC_G04.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;

namespace WNC_G04.Controllers
{
    public class TrangTimKiemController : Controller
    {
        private readonly ILogger<TrangTimKiemController> _logger;
        private readonly DbG04RVContext _context;
        private NguoiDung us;
       
        public TrangTimKiemController(DbG04RVContext context, ILogger<TrangTimKiemController> logger)
        {
            _logger = logger;
            _context = context;
     

        }

        // Phương thức tìm kiếm bài viết
        public IActionResult TrangTimKiem(string? searchQuery)
        {
            // Lấy email của người dùng từ session
            var currentUserEmail = HttpContext.Session.GetString("Email");

            // Lấy thông tin người dùng dựa trên email
            us = _context.NguoiDungs
                .FirstOrDefault(t => t.Email == currentUserEmail);

            // Nếu không có từ khóa tìm kiếm, đặt giá trị mặc định là null
            if (string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = null;
            }

            // Tìm kiếm bài viết chứa từ khóa trong nội dung bài viết hoặc tên người dùng
            var baiViets = _context.BaiViets
                .Where(b =>
                    (b.NoiDung != null && b.NoiDung.Contains(searchQuery)) || // Tìm theo nội dung bài viết
                     (b.MaChuDeNavigation.TenChuDe != null && b.MaChuDeNavigation.TenChuDe.Contains(searchQuery)) ||
                    (b.MaNguoiDungNavigation.TenNguoiDung != null && b.MaNguoiDungNavigation.TenNguoiDung.Contains(searchQuery))) // Tìm theo tên người dùng
                
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
                    IsLiked = _context.Thiches.Any(t => t.MaBaiViet == b.MaBaiViet && t.MaNguoiDung == us.MaNguoiDung),
                    SoLuongLike = b.SoLuongLike
                }).ToList();

            // Trả về view với danh sách bài viết tìm được
            return View(baiViets);
        }
        [HttpPost()]
        public IActionResult LikeEvent(int mabaiviet, int tennguoidung)
        {
            if (mabaiviet == null || tennguoidung == null)
            {
                _logger.LogWarning("mabaiviet hoặc tennguoidung là null");
                return Content("mabaiviet hoặc tennguoidung là null");
            }

            // Hiển thị giá trị của các tham số
            _logger.LogInformation($"mabaiviet: {mabaiviet}, tennguoidung: {tennguoidung}");
            var currentUserEmail = HttpContext.Session.GetString("Email");
            var uss = _context.NguoiDungs
               .FirstOrDefault(t => t.Email == currentUserEmail);
            var existingLike = _context.Thiches
               .FirstOrDefault(t => t.MaBaiViet == mabaiviet && t.MaNguoiDung == uss.MaNguoiDung);
            var baiviet = _context.BaiViets
               .FirstOrDefault(t => t.MaBaiViet == mabaiviet);
            if (existingLike != null)
            {

                baiviet.SoLuongLike--;
                _context.Thiches.Remove(existingLike);
                _context.SaveChanges();
                return Json(new { success = true, newLikeCount = baiviet.SoLuongLike });
            }
            else
            {

                var thich = new Thich
                {
                    MaBaiViet = mabaiviet,
                    MaNguoiDung = uss.MaNguoiDung,
                };
                baiviet.SoLuongLike++;
                _context.Thiches.Add(thich);
                _context.SaveChanges();
                return Json(new { success = true, newLikeCount = baiviet.SoLuongLike });
            }

        }


        [HttpPost]
        public IActionResult AddComment(string comment, int mabaiviet, int tennguoidung)
        {
            var currentUserEmail = HttpContext.Session.GetString("Email");
            var uss = _context.NguoiDungs
               .FirstOrDefault(t => t.Email == currentUserEmail);
            var cmt = new BinhLuan
            {
                MaBaiViet = mabaiviet,
                MaNguoiDung = uss.MaNguoiDung,
                NoiDung = comment
            };

            _context.BinhLuans.Add(cmt);
            _context.SaveChanges();


            return Json(new { success = true, tennguoidung = uss.TenNguoiDung });


        }

    }
}
