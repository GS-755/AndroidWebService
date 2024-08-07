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
    using System.Linq;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using System.Diagnostics;

    public partial class TaiKhoan
    {
        private NguoiDung user;

        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaiKhoan()
        {
            this.StrAvatar = string.Empty;
            this.Base64Avatar = string.Empty;
            this.GiaoDich = new HashSet<GiaoDich>();
            this.NguoiDung = new HashSet<NguoiDung>();
            this.PTYeuThich = new HashSet<PTYeuThich>();
            this.PhongTro = new HashSet<PhongTro>();
        }
        
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string Email { get; set; }
        public string SoDT { get; set; }
        public string StrAvatar { get; set; }
        [NotMapped]
        public string Base64Avatar { get; set; }
        public int MaVaiTro { get; set; }
        public NguoiDung User
        {
            get
            {
                try
                {
                    this.user = this.GetUser();

                    return this.user;
                }
                catch
                {
                    return null;
                }
            }
            set => this.user = value; 
        }
    
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<GiaoDich> GiaoDich { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<NguoiDung> NguoiDung { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]    
        public virtual ICollection<PTYeuThich> PTYeuThich { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<PhongTro> PhongTro { get; set; }
        public virtual VaiTro VaiTro { get; set; }

        private NguoiDung GetUser()
        {
            try
            {
                if (this.NguoiDung != null && this.NguoiDung.Count > 0) 
                {
                    return this.NguoiDung.ToList().FirstOrDefault(
                        k => k.TenDangNhap.Trim() == this.TenDangNhap.Trim()
                    );
                }   
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null; 
        }
    }
}
