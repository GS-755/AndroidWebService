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
    
    public partial class NguoiDung
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NguoiDung()
        {
            this.TaiKhoan = new HashSet<TaiKhoan>();
        }
    
        public string CCCD { get; set; }
        public string Ho { get; set; }
        public string Ten { get; set; }
        public DateTime NgaySinh { get; set; }
        public Nullable<int> GioiTinh { get; set; }
        public string DiaChi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<TaiKhoan> TaiKhoan { get; set; }
    }
}
