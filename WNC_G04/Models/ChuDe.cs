using System.ComponentModel.DataAnnotations.Schema;
using WNC_G04.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WNC_G04.Models

{
    public partial class ChuDe
    {

        public int MaChuDe { get; set; }

        public string TenChuDe { get; set; } = null!;

        [StringLength(10, ErrorMessage = "VerifyKey phải có độ dài tối đa 10 ký tự.")]
        [RegularExpression(@"^\d.*$", ErrorMessage = "VerifyKey phải bắt đầu bằng một chữ số.")]
        public string VerifyKey { get; set; } = null!;

        public virtual ICollection<BaiViet> BaiViets { get; set; } = new List<BaiViet>();
    }
}
