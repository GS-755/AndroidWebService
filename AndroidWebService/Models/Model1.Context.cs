﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DoAnAndroidEntities : DbContext
    {
        public DoAnAndroidEntities()
            : base("name=DoAnAndroidEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<GiaoDich> GiaoDich { get; set; }
        public virtual DbSet<LoaiGiaoDich> LoaiGiaoDich { get; set; }
        public virtual DbSet<NguoiDung> NguoiDung { get; set; }
        public virtual DbSet<PTYeuThich> PTYeuThich { get; set; }
        public virtual DbSet<PhongTro> PhongTro { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoan { get; set; }
        public virtual DbSet<TTPhongTro> TTPhongTro { get; set; }
        public virtual DbSet<VaiTro> VaiTro { get; set; }
        public virtual DbSet<ViTri> ViTri { get; set; }
        public virtual DbSet<TTGiaoDich> TTGiaoDich { get; set; }
    }
}
