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
    using System.Collections.Generic;
    
    public partial class ViTri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ViTri()
        {
            this.PhongTro = new HashSet<PhongTro>();
        }
    
        public int MaVT { get; set; }
        public string Quan { get; set; }
        public Nullable<double> KinhDo { get; set; }
        public Nullable<double> ViDo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhongTro> PhongTro { get; set; }
    }
}
