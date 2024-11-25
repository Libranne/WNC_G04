using System.ComponentModel.DataAnnotations.Schema;
using WNC_G04.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace WNC_G04.Models
{
    public partial class NguoiDung
    {

        public int MaNguoiDung { get; set; }
        [Required(ErrorMessage = "Tên người dùng không được trống.")]
        public string TenNguoiDung { get; set; } = null!;
        [Required(ErrorMessage = "Email không được trống.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được trống.")]
        public string MatKhau { get; set; } = null!;

        [Required(ErrorMessage = "Cần xác nhận mật khẩu.")]
        [Compare("MatKhau", ErrorMessage = "Không trùng khớp với mật khẩu.")]
        public  string ConfirmMatKhau { get; set; }

        public string? AnhDaiDien { get; set; }

        public string? TieuSu { get; set; }

        public DateTime? NgayTao { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LockoutEnd { get; set; } // Thời gian khóa tài khoản

        public virtual ICollection<BaiViet> BaiViets { get; set; } = new List<BaiViet>();

        public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

        public virtual ICollection<TheoDoi> TheoDoiMaNguoiDuocTheoDoiNavigations { get; set; } = new List<TheoDoi>();

        public virtual ICollection<TheoDoi> TheoDoiMaNguoiTheoDoiNavigations { get; set; } = new List<TheoDoi>();

        public virtual ICollection<Thich> Thiches { get; set; } = new List<Thich>();
        public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
    }
}
