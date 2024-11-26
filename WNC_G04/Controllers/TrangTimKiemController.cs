using Microsoft.AspNetCore.Mvc;
using WNC_G04.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;

namespace WNC_G04.Controllers
{
    public class TrangTimKiemController : Controller
    {
        private readonly DbG04RVContext _context;
        private NguoiDung us;

        public TrangTimKiemController(DbG04RVContext context)
        {
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

            // Nếu không có từ khóa tìm kiếm, đặt giá trị mặc định là "2" (có thể thay đổi tùy theo yêu cầu)
            if (string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = "2";
            }

            // Tìm kiếm bài viết chứa từ khóa trong nội dung bài viết
            var baiViets = _context.BaiViets
                .Where(b => b.NoiDung.Contains(searchQuery))
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
    }
}
