USE master; 
IF EXISTS (
	SELECT *
	FROM sys.databases 
	WHERE name = N'DoAnAndroid'
) BEGIN 
	DROP DATABASE DoAnAndroid;
END;
CREATE DATABASE DoAnAndroid;

USE DoAnAndroid;
CREATE TABLE ViTri (
  MaVT INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
  Quan NVARCHAR(25) NOT NULL,
  KinhDo FLOAT,
  ViDo FLOAT
); 
INSERT INTO ViTri(Quan) VALUES
	(N'Quận 10'),
	(N'Quận Tân Bình'),
	(N'Huyện Hóc Môn');
CREATE TABLE NguoiDung (
  CCCD CHAR(15) NOT NULL PRIMARY KEY,
  Ho NVARCHAR(30) NOT NULL,
  Ten NVARCHAR(15) NOT NULL,
  NgaySinh DATE NOT NULL,
  GioiTinh INT,
  DiaChi NVARCHAR(100) NOT NULL
); 
CREATE TABLE VaiTro (
  MaVaiTro INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
  TenVaiTro NVARCHAR(25) NOT NULL
);
INSERT INTO VaiTro VALUES
	(N'Chủ trọ'),
	(N'Khách hàng'),
	(N'Admin');
CREATE TABLE TrangThai (
  MaTT INT IDENTITY (1,1) NOT NULL,
  TenTT VARCHAR(25) NOT NULL,
  PRIMARY KEY (MaTT)
);
INSERT INTO TrangThai VALUES
	(N'Còn trống'), 
	(N'Đã đặt cọc'),
	(N'Đang cho thuê');
CREATE TABLE TaiKhoan (
  TenDangNhap CHAR(16) NOT NULL PRIMARY KEY,
  MatKhau VARCHAR(64) NOT NULL,
  Email CHAR(75),
  SoDT CHAR(15) NOT NULL,
  MaVaiTro INT NOT NULL,
  CCCD CHAR(15) NOT NULL,
  FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro),
  FOREIGN KEY (CCCD) REFERENCES NguoiDung(CCCD)
);
CREATE TABLE PhongTro (
  MaPT INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
  TieuDe NVARCHAR(75) NOT NULL,
  HinhAnh NVARCHAR(MAX) NOT NULL,
  NgayDang DATE NOT NULL,
  MoTa NVARCHAR(100),
  DienTich INT NOT NULL,
  Gia INT NOT NULL,
  TenDangNhap CHAR(16) NOT NULL,
  MaVT INT NOT NULL,
  MaTT INT NOT NULL
  FOREIGN KEY (TenDangNhap) REFERENCES TaiKhoan(TenDangNhap),
  FOREIGN KEY (MaVT) REFERENCES ViTri(MaVT),
  FOREIGN KEY (MaTT) REFERENCES TrangThai(MaTT)
);
CREATE TABLE CTDatPhong (
  NgayDat DATE NOT NULL,
  GhiChu NVARCHAR(100),
  TienCoc FLOAT NOT NULL,
  MaPT INT NOT NULL,
  TenDangNhap CHAR(16) NOT NULL,
  PRIMARY KEY (MaPT, TenDangNhap),
  FOREIGN KEY (MaPT) REFERENCES PhongTro(MaPT),
  FOREIGN KEY (TenDangNhap) REFERENCES TaiKhoan(TenDangNhap)
);

-- Emergency case only (use with Pointer for ease)
CREATE PROCEDURE sp_changeimg_phongtro_01 @MaPT INT, @HinhAnh NVARCHAR(MAX)
AS BEGIN
	IF EXISTS (
		SELECT *
		FROM PhongTro
		WHERE MaPT = @MaPT
	) 
	BEGIN
		UPDATE PhongTro
			SET HinhAnh = @HinhAnh 
			WHERE MaPT = @MaPT
		PRINT(N'Đã cập nhật ảnh cho phòng trọ #' + @MaPT);
	END;
END;