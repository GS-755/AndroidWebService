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

-- Khu vực bảng Tài khoản
CREATE TABLE VaiTro (
	MaVaiTro INT NOT NULL PRIMARY KEY,
	TenVaiTro NVARCHAR(25) NOT NULL UNIQUE 
);
INSERT INTO VaiTro VALUES
	(1, N'Chủ trọ'),
	(0, N'Khách hàng');
CREATE TABLE TaiKhoan (
	TenDangNhap CHAR(16) NOT NULL PRIMARY KEY,
	MatKhau VARCHAR(64) NOT NULL,
	Email CHAR(75) NOT NULL UNIQUE,
	SoDT CHAR(15) NOT NULL UNIQUE,
	StrAvatar NVARCHAR(MAX), 
	MaVaiTro INT NOT NULL,
	FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);
CREATE TABLE NguoiDung (
	CCCD CHAR(15) NOT NULL PRIMARY KEY,
	Ho NVARCHAR(30) NOT NULL,
	Ten NVARCHAR(15) NOT NULL,
	NgaySinh DATE,
	GioiTinh INT, 
	-- ISO 0 - 1 - 2 - 9: 
	-- 0: Nam 
	-- 1: Nữ 
	-- 2: LGBTQ+ 
	-- 9: N/A
	DiaChi NVARCHAR(100) NOT NULL, 
	TenDangNhap CHAR(16), 
	FOREIGN KEY(TenDangNhap) REFERENCES TaiKhoan(TenDangNhap)
); 
-- Khu vực bảng Phòng trọ
CREATE TABLE ViTri (
	MaVT INT NOT NULL PRIMARY KEY,
	Quan NVARCHAR(25) NOT NULL UNIQUE,
	KinhDo FLOAT,
	ViDo FLOAT
); 
INSERT INTO ViTri(MaVT, Quan) VALUES
	(1, N'Quận 10'),
	(2, N'Quận 12'), 
	(3, N'Quận Tân Bình'), 
	(4, N'Quận Tân Phú'), 
	(5, N'Huyện Hóc Môn');
CREATE TABLE TTPhongTro (
  MaTT INT NOT NULL PRIMARY KEY,
  TenTT NVARCHAR(25) NOT NULL UNIQUE,
);
INSERT INTO TTPhongTro VALUES
  (0, N'Đã ẩn'),
  (1, N'Đang cho thuê'), 
  (2, N'Đã đặt cọc'),
  (3, N'Đã cho thuê');
CREATE TABLE PhongTro (
  MaPT INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
  TieuDe NVARCHAR(75) NOT NULL,
  NgayDang DATE NOT NULL,
  DienTich FLOAT NOT NULL,
  SoTien FLOAT NOT NULL,
  TienCoc FLOAT, 
  MoTa NVARCHAR(MAX),
  HinhAnh NVARCHAR(MAX),
  DiaChi NVARCHAR(200) NOT NULL, 
  TenDangNhap CHAR(16) NOT NULL,
  MaVT INT NOT NULL,
  MaTT INT NOT NULL, 
  FOREIGN KEY (TenDangNhap) REFERENCES TaiKhoan(TenDangNhap),
  FOREIGN KEY (MaVT) REFERENCES ViTri(MaVT),
  FOREIGN KEY (MaTT) REFERENCES TTPhongTro(MaTT)
);
CREATE TABLE LoaiMedia (
	MaLoaiMedia INT NOT NULL PRIMARY KEY, 
	TenLoaiMedia NVARCHAR(25) NOT NULL UNIQUE
); 
INSERT INTO LoaiMedia VALUES 
	(1, N'Hình ảnh'), 
	(2, N'Video');
CREATE TABLE MotelMedia (
	MaPT INT NOT NULL, 
	MaMedia INT NOT NULL IDENTITY(1, 1), 
	TenTepMedia NVARCHAR(MAX) NOT NULL, 
	MaLoaiMedia INT NOT NULL, 
	PRIMARY KEY(MaPT, MaMedia), 
	FOREIGN KEY(MaPT) REFERENCES PhongTro(MaPT), 
	FOREIGN KEY(MaLoaiMedia) REFERENCES LoaiMedia(MaLoaiMedia)
);
CREATE TABLE PTYeuThich (
  GhiChu NVARCHAR(100), 
  MaPT INT NOT NULL, 
  TenDangNhap CHAR(16) NOT NULL, 
  PRIMARY KEY(MaPT, TenDangNhap), 
  FOREIGN KEY(MaPT) REFERENCES PhongTro(MaPT), 
  FOREIGN KEY(TenDangNhap) REFERENCES TaiKhoan(TenDangNhap)
);

-- Khu vực bảng Giao dịch
CREATE TABLE LoaiGiaoDich (
	MaLoaiGD INT IDENTITY(1, 1) PRIMARY KEY, 
	TenLoaiGD NVARCHAR(20) NOT NULL UNIQUE
);
INSERT INTO LoaiGiaoDich VALUES (N'Đặt cọc');
INSERT INTO LoaiGiaoDich VALUES (N'Chuyển toàn bộ');
CREATE TABLE TTGiaoDich (
	MaTTGD SMALLINT NOT NULL PRIMARY KEY, 
	TenTTGD NVARCHAR(25) NOT NULL UNIQUE 
); 
INSERT INTO TTGiaoDich VALUES(0, N'Thất bại');
INSERT INTO TTGiaoDich VALUES(1, N'Thành công');
CREATE TABLE GiaoDich (
	MaGD CHAR(8) NOT NULL PRIMARY KEY, 
	MaLoaiGD INT NOT NULL, 
	MaPT INT NOT NULL, 
	NgayGD DATE NOT NULL, 
	SoTien FLOAT NOT NULL, 
	MaTTGD SMALLINT, 
	TenDangNhap CHAR(16) NOT NULL, 
	FOREIGN KEY(MaPT) REFERENCES PhongTro(MaPT), 
	FOREIGN KEY(MaLoaiGD) REFERENCES LoaiGiaoDich(MaLoaiGD), 
	FOREIGN KEY(MaTTGD) REFERENCES TTGiaoDich(MaTTGD), 
	FOREIGN KEY(TenDangNhap) REFERENCES TaiKhoan(TenDangNhap) 
);

-- Trigger(s)
-- Calculate deposit 
CREATE OR ALTER TRIGGER tg_calcdeposit_r1 ON PhongTro 
FOR INSERT, UPDATE AS BEGIN 
	DECLARE @maPT INT; 
	DECLARE @oldAmount FLOAT;
	SELECT @maPT = (
		SELECT MaPT
		FROM inserted
	); 
	SELECT @oldAmount = (
		SELECT SoTien
		FROM inserted
	);
	UPDATE PhongTro 
		SET TienCoc = @oldAmount * 30 / 100
		WHERE MaPT = @maPT;
END; 
-- Prevent deleting Transaction informations 
CREATE OR ALTER TRIGGER tg_deltransaction_r2 ON GiaoDich 
FOR UPDATE, DELETE AS BEGIN 
	Rollback Transaction; 
	Raiserror(N'KHÔNG ĐƯỢC PHÉP xoá HOẶC chỉnh sửa giao dịch', 16, 1);
END;
-- Prevent deleting User informations 
CREATE OR ALTER TRIGGER tg_deluser_r3 ON NguoiDung 
FOR DELETE AS BEGIN 
	Rollback Transaction; 
	Raiserror(N'KHÔNG ĐƯỢC PHÉP xoá dữ liệu người dùng', 16, 1);
END;
-- Prevent deleting User accounts 
CREATE OR ALTER TRIGGER tg_delaccount_r4 ON TaiKhoan 
FOR DELETE AS BEGIN 
	Rollback Transaction; 
	Raiserror(N'KHÔNG ĐƯỢC PHÉP xoá tài khoản người dùng', 16, 1);
END; 
-- Assign default profile picture for non-updated avatar account 
CREATE OR ALTER TRIGGER tg_setdefaultavatar_r5 ON TaiKhoan 
FOR INSERT, UPDATE AS BEGIN 
	DECLARE @tenDangNhap CHAR(16); 
	DECLARE @strAvatar NVARCHAR(MAX); 
	SELECT @tenDangNhap = (
		SELECT TenDangNhap 
		FROM inserted
	); 
	SET @strAvatar = N'defaultProfilePicture.png';
	UPDATE TaiKhoan 
		SET StrAvatar = @strAvatar 
		WHERE TenDangNhap = @tenDangNhap; 
END;

-- Stored procedure(s)
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

-- Show the list of triggers 
SELECT * 
FROM sys.triggers;
-- Show the list of SP(s)
SELECT * 
FROM sys.procedures;
