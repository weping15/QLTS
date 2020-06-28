create database QuanLyTraSua
go
use QuanLyTraSua
go
create table tblChatLieu
( 
	MaChatLieu nvarchar(50) not null,
	TenChatLieu nvarchar(50) null,
	primary key clustered(MaChatLieu asc)
	--tao khoa chinh
)
go
create table tblKhachHang(
	MaKhachHang nvarchar(10) not null,
	TenKhachHang nvarchar (50) not null,
	DiaChi nvarchar (50) not null,
	SDT nvarchar (50) not null,
	primary key clustered(MaKhachHang asc)
)
go
create table tblHang
(	
	MaHang nvarchar (50) not null,
	TenHang  nvarchar (50) not null,
	MaChatLieu nvarchar (50) not null,
	SoLuong float(53) not null,
	DonGiaNhap float(53) not null,
	DonGiaBan float(53) not null,
	GhiChu nvarchar(200) null,
	primary key clustered(MaHang asc),
	constraint FK_Hang FOREIGN KEY (MaChatLieu) references dbo.tblChatLieu (MaChatLieu)
)
go
create table tblNhanVien(
	MaNhanVien nvarchar (50) not null,
	TenNhanVien nvarchar (50) not null,
	GioiTinh nvarchar (10) not null,
	DiaChi nvarchar (200) not null,
	SDT nvarchar (15) not null,
	NgaySinh datetime not null,
	primary key clustered(MaNhanVien asc)
)
go
create table tblHoaDon
(	
	MaHoaDon nvarchar (30) not null,
	MaNhanVien nvarchar (50) not null,
	NgayBan datetime not null,
	MaKhachHang nvarchar (10) not null,
	TongTien float(53) not null,
	primary key clustered (MaHoaDon asc),
	constraint FK_HD_MNV FOREIGN KEY (MaNhanVien) references  dbo.tblNhanVien(MaNhanVien),
	constraint FK_HD_MKH FOREIGN KEY (MaKhachHang) references dbo.tblKhachHang (MaKhachHang)
)
go
create table tblChiTietHoaDon
(	
	MaHoaDon nvarchar (30) not null,
	MaHang nvarchar (50) not null,
	SoLuong float(53) not null,
	DonGia float(53) not null,
	GiamGia float(53) not null,
	ThanhTien float(53) not null,
	primary key clustered (MaHoaDon asc,MaHang asc),
	constraint FK_CTHD_MH FOREIGN KEY (MaHang) references dbo.tblHang (MaHang),
	constraint FK_CTHD_HD FOREIGN KEY (MaHoaDon) references dbo.tblHoaDon (MaHoaDon)

)
go
