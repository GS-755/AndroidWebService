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
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public partial class ViTri
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ViTri()
        {
            this.PhongTro = new HashSet<PhongTro>();
            this.HinhAnh = string.Empty;
        }
    
        public int MaVT { get; set; }
        public string Quan { get; set; }
        public Nullable<double> KinhDo { get; set; }
        public Nullable<double> ViDo { get; set; }
        public string HinhAnh { get; set; }
    
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<PhongTro> PhongTro { get; set; }
    }
}
