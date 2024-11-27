using Microsoft.AspNetCore.Mvc;
using WNC_G04.Models;
using System.Linq;

namespace WNC_G04.Controllers
{
    public class ThongBaoController : Controller
    {
        private readonly ILogger<TrangchuController> _logger;
        private readonly DbG04RVContext _context;
        private NguoiDung us;

        public ThongBaoController(DbG04RVContext context)
        {
            _context = context;
        }

        // Hành động để lấy danh sách thông báo
        public IActionResult Index()
        {
            var currentUserEmail = HttpContext.Session.GetString("Email");
            var us = _context.NguoiDungs
                .FirstOrDefault(t => t.Email == currentUserEmail);

            var thongbao = _context.ThongBaos
                .Where(t => t.MaNguoiDung == us.MaNguoiDung)  // Lọc thông báo theo người dùng
                .Select(t => new ThongBao
                {
                    MaThongBao = t.MaThongBao,
                    MaNguoiDung = t.MaNguoiDung,
                    NoiDung = t.NoiDung,
                    NgayTao = t.NgayTao ?? DateTime.Now,
                    LoaiThongBao = t.LoaiThongBao,
                    MaBaiViet = t.MaBaiViet
                }).ToList();

            return View(thongbao);
        }





        // Hành động để đánh dấu thông báo là đã đọc
        public IActionResult MarkAsRead(int id)
        {
            var thongBao = _context.ThongBaos.FirstOrDefault(t => t.MaThongBao == id);
            if (thongBao != null)
            {
                thongBao.IsRead = true; // Đánh dấu là đã đọc
                _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            }

            return RedirectToAction("Index"); // Trở về trang thông báo sau khi cập nhật
        }




        // Hành động để xóa thông báo
        public IActionResult Delete(int id)
        {
            var thongBao = _context.ThongBaos.Find(id);
            if (thongBao != null)
            {
                _context.ThongBaos.Remove(thongBao); // Xóa thông báo
                _context.SaveChanges();
            }

            return RedirectToAction("Index"); // Quay lại danh sách thông báo
        }

    }
}
