﻿namespace WNC_G04.Models
{
    public partial class Thich
    {
        public int MaThich { get; set; }

        public int? MaBaiViet { get; set; }

        public int? MaNguoiDung { get; set; }

        public virtual BaiViet? MaBaiVietNavigation { get; set; }

        public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
    }
}
