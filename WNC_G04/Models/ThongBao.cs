using System.ComponentModel.DataAnnotations.Schema;
using WNC_G04.Models;
using System;
using System.Collections.Generic;
namespace WNC_G04.Models
{
    public partial class ThongBao
    {
        public int MaThongBao { get; set; }  // Khóa chính của bảng ThongBao

        public int? MaNguoiDung { get; set; }  // Khóa ngoại liên kết đến người dùng nhận thông báo

        public string NoiDung { get; set; } = null!;  // Nội dung thông báo, không cho phép null

        public DateTime? NgayTao { get; set; }  // Ngày tạo thông báo, có thể null


        // Thông tin thêm về loại thông báo (thích hay bình luận)
        public string? LoaiThongBao { get; set; }  // "Thich" hoặc "BinhLuan"

        public int? MaBaiViet { get; set; }  // Khóa ngoại liên kết tới bài viết có liên quan
        public bool IsRead { get; set; } = false; // Trạng thái đã đọc

        public virtual BaiViet? MaBaiVietNavigation { get; set; }  // Liên kết đến bài viết
        public virtual NguoiDung? MaNguoiDungNavigation { get; set; }  // Liên kết đến thực thể NguoiDung
    }
}
