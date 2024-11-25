using System.ComponentModel.DataAnnotations.Schema;
using WNC_G04.Models;
using System;
using System.Collections.Generic;

namespace WNC_G04.Models

{
    public partial class ChuDe
    {

        public int MaChuDe { get; set; }

        public string TenChuDe { get; set; } = null!;

        public virtual ICollection<BaiViet> BaiViets { get; set; } = new List<BaiViet>();
    }
}
