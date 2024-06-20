//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AndroidWebService.Models
{
    using System;
    using AndroidWebService.Models.Enums;

    public partial class GiaoDich
    {
        public string MaGD { get; set; }
        public int MaLoaiGD { get; set; }
        public int MaPT { get; set; }
        public DateTime NgayGD { get; set; }
        public double SoTien { get; set; }
        public Nullable<short> MaTTGD { get; set; }
        public string TenDangNhap { get; set; }
        
        public GiaoDich()
        {
            this.NgayGD = DateTime.Now; 
            this.MaTTGD = (short)TransactionStatus.Pending;
        }

        public virtual LoaiGiaoDich LoaiGiaoDich { get; set; }
        public virtual PhongTro PhongTro { get; set; }
        public virtual TTGiaoDich TTGiaoDich { get; set; }
        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
