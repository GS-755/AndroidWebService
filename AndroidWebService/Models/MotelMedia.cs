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

    public partial class MotelMedia
    {
        public int MaPT { get; set; }
        public int MaMedia { get; set; }
        public string TenTepMedia { get; set; }
        public int MaLoaiMedia { get; set; }
    
        public virtual LoaiMedia LoaiMedia { get; set; }
        [JsonIgnore]
        public virtual PhongTro PhongTro { get; set; }
    }
}
