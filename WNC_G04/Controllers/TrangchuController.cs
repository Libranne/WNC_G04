﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WNC_G04.Models;

namespace WNC_G04.Controllers
{
    public class TrangchuController : Controller
    {
        private readonly ILogger<TrangchuController> _logger;
        private readonly DbG04RVContext _context;
        private NguoiDung us;
        public TrangchuController(DbG04RVContext context, ILogger<TrangchuController> logger)
        {
            _logger = logger;
            _context = context;

        }

        [HttpGet]
        public IActionResult ListBaiViet()
        {
            var currentUserEmail = HttpContext.Session.GetString("Email");
            us = _context.NguoiDungs
               .FirstOrDefault(t => t.Email == currentUserEmail);

            var baiViets = _context.BaiViets
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

            var cmts = _context.BinhLuans.Select(b => new BinhLuan
            {
                MaBinhLuan = b.MaBinhLuan,
                MaBaiViet = b.MaBaiViet,
                MaNguoiDung = b.MaNguoiDung,
                TenNguoiDung = b.MaNguoiDungNavigation.TenNguoiDung,
                NoiDung = b.NoiDung,
                NgayTao = b.NgayTao



            }).ToList();

            ViewBag.us = us; // Pass the current user ID to the view
            ViewBag.cmts = cmts;
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

            if (baiviet.MaNguoiDung != uss.MaNguoiDung)
            {
               var thongBao = new ThongBao
               {
                    MaNguoiDung = baiviet.MaNguoiDung,
                    NoiDung = $"{uss.TenNguoiDung} đã thích bài viết của bạn.",
                    NgayTao = DateTime.Now,
                    LoaiThongBao = "Thich",
                    MaBaiViet = mabaiviet
               };
               _context.ThongBaos.Add(thongBao);
            }
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

            //
        var baiviet = _context.BaiViets
              .FirstOrDefault(b => b.MaBaiViet == mabaiviet);
        if (baiviet == null || uss == null)
                return Json(new { success = false, message = "Bài viết hoặc người dùng không tồn tại." });




        var cmt = new BinhLuan
        {
            MaBaiViet = mabaiviet,
            MaNguoiDung = uss.MaNguoiDung,
            NoiDung = comment
        };

        _context.BinhLuans.Add(cmt);

            if (baiviet.MaNguoiDung != uss.MaNguoiDung)
            {
                var thongBao = new ThongBao
                {
                    MaNguoiDung = baiviet.MaNguoiDung,
                    NoiDung = $"{uss.TenNguoiDung} đã bình luận về bài viết của bạn.",
                    NgayTao = DateTime.Now,
                    LoaiThongBao = "BinhLuan",
                    MaBaiViet = mabaiviet
                };
                _context.ThongBaos.Add(thongBao);
            }

            _context.SaveChanges();


        return Json(new { success = true, tennguoidung = uss.TenNguoiDung });

    }
        

    }
}
