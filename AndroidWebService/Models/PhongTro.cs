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
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class PhongTro
    {
        public static readonly string SERVER_IMG_PATH = "~/Resources/Pictures/";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhongTro()
        {
            this.PTYeuThich = new HashSet<PTYeuThich>();
            this.GiaoDich = new HashSet<GiaoDich>();
            this.NgayDang = DateTime.Now;
        }
    
        public int MaPT { get; set; }
        public string TieuDe { get; set; }
        public DateTime NgayDang { get; set; }
        public double DienTich { get; set; }
        public double SoTien { get; set; }
        public Nullable<double> TienCoc { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        [NotMapped]
        [JsonIgnore]
        public HttpPostedFileBase UploadImage { get; set; }
        public string TenDangNhap { get; set; }
        public int MaVT { get; set; }
        public int MaTT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<PTYeuThich> PTYeuThich { get; set; }
        [JsonIgnore]
        public virtual TaiKhoan TaiKhoan { get; set; }
        [JsonIgnore]
        public virtual TTPhongTro TTPhongTro { get; set; }
        [JsonIgnore]
        public virtual ViTri ViTri { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<GiaoDich> GiaoDich { get; set; }
    }
}
