using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WNC_G04.Models;

namespace WNC_G04.Controllers
{
    public class KhamPhaController : Controller
    {
        private readonly DbG04RVContext _context;
        private NguoiDung us;

        public KhamPhaController(DbG04RVContext context)
        {
            _context = context;
        }

        public IActionResult GetPostsByTopic(int MaChuDe)
        {
            var currentUserEmail = HttpContext.Session.GetString("Email");
            var currentUser = _context.NguoiDungs.FirstOrDefault(t => t.Email == currentUserEmail);

            if (currentUser == null) return Json(new { success = false });

            var baiViets = _context.BaiViets
                .Where(b => b.MaChuDe == MaChuDe)
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
                    IsLiked = _context.Thiches.Any(t => t.MaBaiViet == b.MaBaiViet && t.MaNguoiDung == currentUser.MaNguoiDung),
                    SoLuongLike = b.SoLuongLike
                }).ToList();

            // Trả về một phần của view (partial view) chứa các bài viết của topic
            return PartialView("_PostListPartial", baiViets);
        }

        public IActionResult Index(int? MaChuDe)
        {
            var currentUserEmail = HttpContext.Session.GetString("Email");
            us = _context.NguoiDungs.FirstOrDefault(t => t.Email == currentUserEmail);

            var topics = _context.ChuDes.ToList();
            ViewBag.Topics = topics;

            if (MaChuDe == null && topics.Count > 0)
            {
                MaChuDe = topics.First().MaChuDe;
            }

            var baiViets = _context.BaiViets
                .Where(b => b.MaChuDe == MaChuDe)
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
                    IsLiked = _context.Thiches.Any(t => t.MaBaiViet == b.MaBaiViet && t.MaNguoiDung == us.MaNguoiDung),
                    SoLuongLike = b.SoLuongLike
                }).ToList();

            var comments = _context.BinhLuans
                .Select(c => new BinhLuan
                {
                    MaBinhLuan = c.MaBinhLuan,
                    MaBaiViet = c.MaBaiViet,
                    MaNguoiDung = c.MaNguoiDung,
                    TenNguoiDung = c.MaNguoiDungNavigation.TenNguoiDung,
                    NoiDung = c.NoiDung,
                    NgayTao = c.NgayTao
                }).ToList();

            ViewBag.SelectedTopicID = MaChuDe;
            ViewBag.us = us;
            ViewBag.cmts = comments;

            return View(baiViets);
        }

        [HttpPost]
        public IActionResult LikePost(int mabaiviet)
        {
            var currentUserEmail = HttpContext.Session.GetString("Email");
            var currentUser = _context.NguoiDungs.FirstOrDefault(t => t.Email == currentUserEmail);
            var baiviet = _context.BaiViets.FirstOrDefault(t => t.MaBaiViet == mabaiviet);

            if (baiviet == null || currentUser == null) return Json(new { success = false });

            var existingLike = _context.Thiches.FirstOrDefault(t => t.MaBaiViet == mabaiviet && t.MaNguoiDung == currentUser.MaNguoiDung);

            if (existingLike != null)
            {
                baiviet.SoLuongLike--;
                _context.Thiches.Remove(existingLike);

                var thongBao = _context.ThongBaos.FirstOrDefault(t => t.MaNguoiDung == baiviet.MaNguoiDung && t.MaBaiViet == mabaiviet && t.LoaiThongBao == "Thich");
                if (thongBao != null)
                {
                    _context.ThongBaos.Remove(thongBao);
                }
            }
            else
            {
                var like = new Thich
                {
                    MaBaiViet = mabaiviet,
                    MaNguoiDung = currentUser.MaNguoiDung
                };
                baiviet.SoLuongLike++;
                _context.Thiches.Add(like);

                if (baiviet.MaNguoiDung != currentUser.MaNguoiDung)
                {
                    var thongBao = new ThongBao
                    {
                        MaNguoiDung = baiviet.MaNguoiDung,
                        NoiDung = $"{currentUser.TenNguoiDung} đã thích bài viết của bạn.",
                        NgayTao = DateTime.Now,
                        LoaiThongBao = "Thich",
                        MaBaiViet = mabaiviet
                    };
                    _context.ThongBaos.Add(thongBao);
                }
            }

            _context.SaveChanges();

            return Json(new
            {
                success = true,
                newLikeCount = baiviet.SoLuongLike
            });
        }

        [HttpPost]
        public IActionResult AddComment(int mabaiviet, string comment)
        {
            var currentUserEmail = HttpContext.Session.GetString("Email");
            var currentUser = _context.NguoiDungs
                .FirstOrDefault(t => t.Email == currentUserEmail);
            var baiviet = _context.BaiViets.FirstOrDefault(b => b.MaBaiViet == mabaiviet);

            if (baiviet == null || currentUser == null)
                return Json(new { success = false, message = "Bài viết hoặc người dùng không tồn tại." });

            try
            {
                var cmt = new BinhLuan
                {
                    MaBaiViet = mabaiviet,
                    MaNguoiDung = currentUser.MaNguoiDung,
                    NoiDung = comment,
                    NgayTao = DateTime.Now
                };

                _context.BinhLuans.Add(cmt);

                if (baiviet.MaNguoiDung != currentUser.MaNguoiDung)
                {
                    var thongBao = new ThongBao
                    {
                        MaNguoiDung = baiviet.MaNguoiDung,
                        NoiDung = $"{currentUser.TenNguoiDung} đã bình luận về bài viết của bạn.",
                        NgayTao = DateTime.Now,
                        LoaiThongBao = "BinhLuan",
                        MaBaiViet = mabaiviet
                    };
                    _context.ThongBaos.Add(thongBao);
                }

                _context.SaveChanges();

                return Json(new { success = true, tennguoidung = currentUser.TenNguoiDung });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }


        public IActionResult GetNotifications()
        {
            var currentUserEmail = HttpContext.Session.GetString("Email");
            var currentUser = _context.NguoiDungs.FirstOrDefault(t => t.Email == currentUserEmail);

            if (currentUser == null) return Json(new { success = false });

            var notifications = _context.ThongBaos
                .Where(t => t.MaNguoiDung == currentUser.MaNguoiDung)
                .OrderByDescending(t => t.NgayTao)
                .ToList();

            return Json(notifications);
        }
    }
}
