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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public partial class VaiTro
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VaiTro()
        {
            this.TaiKhoan = new HashSet<TaiKhoan>();
        }
    
        public int MaVaiTro { get; set; }
        public string TenVaiTro { get; set; }
    
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<TaiKhoan> TaiKhoan { get; set; }
    }
}
