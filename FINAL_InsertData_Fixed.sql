-- ✅ Sửa: Thêm LoaiMayBay (bỏ IDENTITY lỗi)
INSERT INTO LoaiMayBay (SoGhe, MoTa) VALUES (150, N'Dòng máy bay LM01');
INSERT INTO LoaiMayBay (SoGhe, MoTa) VALUES (180, N'Dòng máy bay LM02');
INSERT INTO LoaiMayBay (SoGhe, MoTa) VALUES (180, N'Dòng máy bay LM03');
INSERT INTO LoaiMayBay (SoGhe, MoTa) VALUES (150, N'Dòng máy bay LM04');
INSERT INTO LoaiMayBay (SoGhe, MoTa) VALUES (220, N'Dòng máy bay LM05');


-- ⬇ Phần còn lại giữ nguyên:
INSERT INTO MayBay (TenHangHK, LoaiMayBayId) VALUES (N'Hãng HK 1', N'LM02');
INSERT INTO MayBay (TenHangHK, LoaiMayBayId) VALUES (N'Hãng HK 2', N'LM02');
INSERT INTO MayBay (TenHangHK, LoaiMayBayId) VALUES (N'Hãng HK 3', N'LM02');
INSERT INTO MayBay (TenHangHK, LoaiMayBayId) VALUES (N'Hãng HK 4', N'LM01');
INSERT INTO MayBay (TenHangHK, LoaiMayBayId) VALUES (N'Hãng HK 5', N'LM05');
INSERT INTO MayBay (TenHangHK, LoaiMayBayId) VALUES (N'Hãng HK 6', N'LM01');
INSERT INTO MayBay (TenHangHK, LoaiMayBayId) VALUES (N'Hãng HK 7', N'LM05');
INSERT INTO MayBay (TenHangHK, LoaiMayBayId) VALUES (N'Hãng HK 8', N'LM04');
INSERT INTO MayBay (TenHangHK, LoaiMayBayId) VALUES (N'Hãng HK 9', N'LM01');
INSERT INTO MayBay (TenHangHK, LoaiMayBayId) VALUES (N'Hãng HK 10', N'LM01');
INSERT INTO SanBay (TenSanBay, DiaDiem) VALUES (N'Sân bay North Debbiebury', N'East Brandi');
INSERT INTO SanBay (TenSanBay, DiaDiem) VALUES (N'Sân bay Barryville', N'Dunntown');
INSERT INTO SanBay (TenSanBay, DiaDiem) VALUES (N'Sân bay Lake Gordon', N'New Jamie');
INSERT INTO SanBay (TenSanBay, DiaDiem) VALUES (N'Sân bay Port Amyland', N'East Brianshire');
INSERT INTO SanBay (TenSanBay, DiaDiem) VALUES (N'Sân bay Morganbury', N'Whitebury');
INSERT INTO SanBay (TenSanBay, DiaDiem) VALUES (N'Sân bay Ambermouth', N'Kathleenmouth');
INSERT INTO SanBay (TenSanBay, DiaDiem) VALUES (N'Sân bay Kelleyport', N'Warrenhaven');
INSERT INTO SanBay (TenSanBay, DiaDiem) VALUES (N'Sân bay Lake Julie', N'North Audreyside');
INSERT INTO SanBay (TenSanBay, DiaDiem) VALUES (N'Sân bay Joshuatown', N'Thorntontown');
INSERT INTO SanBay (TenSanBay, DiaDiem) VALUES (N'Sân bay East Lucasside', N'New Dana');

INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (2, 4, 10, '2025-01-29 14:02:38', '2025-03-03 21:36:00', 2059806, N'Hoàn thành');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (1, 9, 4, '2025-02-02 11:38:31', '2025-05-02 05:45:30', 2501601, N'Hoàn thành');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (9, 7, 4, '2025-02-04 10:55:41', '2025-06-16 01:53:43', 1942059, N'Hoàn thành');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (5, 1, 3, '2025-03-12 11:16:14', '2025-03-22 14:51:22', 2464104, N'Sắp cất cánh');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (6, 5, 3, '2025-05-13 00:04:13', '2025-02-23 06:22:42', 1451545, N'Sắp cất cánh');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (2, 2, 7, '2025-04-18 20:25:14', '2025-05-09 19:23:20', 1202828, N'Sắp cất cánh');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (6, 10, 5, '2025-07-05 02:55:38', '2025-06-12 14:20:09', 2692670, N'Đang bay');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (8, 9, 2, '2025-04-07 23:42:39', '2025-03-08 09:43:36', 2934193, N'Sắp cất cánh');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (2, 9, 5, '2025-06-21 04:09:12', '2025-06-28 10:08:19', 2739387, N'Hoàn thành');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (10, 6, 4, '2025-02-21 01:15:04', '2025-06-05 04:43:13', 2477594, N'Đang bay');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (1, 4, 5, '2025-02-18 10:05:20', '2025-02-22 14:12:38', 1167335, N'Đang bay');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (2, 7, 5, '2025-05-22 18:00:12', '2025-05-02 10:32:15', 1950870, N'Hoàn thành');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (6, 3, 6, '2025-06-29 20:02:26', '2025-06-18 22:06:14', 1745056, N'Đang bay');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (5, 2, 3, '2025-06-15 00:00:51', '2025-04-02 01:05:00', 2120172, N'Hoàn thành');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (4, 3, 8, '2025-06-08 06:50:19', '2025-06-10 10:00:59', 1795775, N'Sắp cất cánh');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (9, 4, 6, '2025-02-03 10:30:03', '2025-05-19 19:26:47', 2767588, N'Đang bay');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (4, 1, 6, '2025-04-24 19:08:29', '2025-04-19 18:08:44', 1841303, N'Sắp cất cánh');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (2, 4, 6, '2025-07-15 22:10:46', '2025-06-30 12:54:19', 1445910, N'Hoàn thành');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (8, 7, 8, '2025-05-12 22:39:40', '2025-06-05 18:24:00', 1299623, N'Sắp cất cánh');


INSERT INTO ChuyenBay (IDMayBay, SanBayDi, SanBayDen, GioCatCanh, GioHaCanh, GiaVe, TinhTrang)
VALUES (3, 4, 9, '2025-02-17 08:47:19', '2025-02-04 10:26:47', 2130317, N'Sắp cất cánh');

INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (19, N'B29', N'Đã đặt');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (12, N'A5', N'Đã đặt');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (3, N'A28', N'Trống');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (5, N'C6', N'Đã đặt');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (20, N'A13', N'Đã đặt');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (20, N'B17', N'Đã đặt');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (18, N'A22', N'Trống');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (18, N'B25', N'Đã đặt');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (4, N'B14', N'Trống');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (15, N'A24', N'Đã đặt');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (17, N'A17', N'Trống');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (10, N'C17', N'Trống');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (5, N'B25', N'Trống');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (18, N'C30', N'Trống');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (20, N'B16', N'Trống');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (4, N'B29', N'Đã đặt');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (8, N'A8', N'Trống');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (3, N'C16', N'Trống');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (18, N'A5', N'Đã đặt');
INSERT INTO GheNgoi (IDChuyenBay, HangGhe, TrangThai) VALUES (18, N'A9', N'Đã đặt');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user01', N'!@I+pjrft6', N'KhachHang', N'HoatDong');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user02', N'tHUlGOvJ)1', N'Admin', N'ChuaKichHoat');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user03', N'Zz5IPWOu$*', N'Admin', N'ChuaKichHoat');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user04', N'$7USd74LxV', N'Admin', N'HoatDong');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user05', N'ox2JNS$5q^', N'KhachHang', N'HoatDong');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user06', N'HtRv5Fhdi!', N'KhachHang', N'ChuaKichHoat');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user07', N'_&IaGENdA6', N'KhachHang', N'HoatDong');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user08', N')gChFpPx^8', N'KhachHang', N'HoatDong');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user09', N'+jcvQuv5R9', N'KhachHang', N'HoatDong');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user10', N'1(+6YYsipm', N'KhachHang', N'HoatDong');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user11', N'z0An0b^C*p', N'KhachHang', N'ChuaKichHoat');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user12', N'I7ah8yEb)7', N'KhachHang', N'HoatDong');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user13', N'Y&5WTuJ28@', N'Admin', N'ChuaKichHoat');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user14', N'!4FbAq3n70', N'KhachHang', N'HoatDong');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user15', N'T_0(oB&vLM', N'Admin', N'HoatDong');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user16', N'DAo6QCqv&t', N'Admin', N'ChuaKichHoat');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user17', N'%7QuVp2LWM', N'KhachHang', N'HoatDong');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user18', N'@4CElPe&FJ', N'KhachHang', N'ChuaKichHoat');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user19', N'rFF)@3teu%', N'Admin', N'ChuaKichHoat');
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThaiTK) VALUES (N'user20', N'#_X26BuxIb', N'Admin', N'ChuaKichHoat');

INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user01', N'Nicole Davenport', N'nicholas75@gmail.com', N'001-923-914-7215', N'Nam', N'Việt Nam', N'820151617123');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user02', N'Michael Mueller', N'petersrandall@white-sosa.biz', N'439.230.8346', N'Nam', N'Việt Nam', N'542641960943');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user03', N'Monica Kelley', N'paynemichael@hotmail.com', N'(601)230-8417', N'Nữ', N'Việt Nam', N'371052246567');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user04', N'Cassandra Parker', N'hgentry@yahoo.com', N'964.289.1520x9446', N'Nam', N'Việt Nam', N'689227460591');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user05', N'Alicia Hobbs', N'william96@ho.com', N'+1-518-591-5667x912', N'Nữ', N'Việt Nam', N'564458546356');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user06', N'Felicia Austin', N'morenoemily@leach-parker.info', N'855-653-7561', N'Nam', N'Việt Nam', N'608002483225');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user07', N'Kathy Odonnell', N'robert54@gmail.com', N'+1-778-133-3737x350', N'Nam', N'Việt Nam', N'585655080082');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user08', N'Ray Clark', N'alexandra99@martinez.com', N'4110612552', N'Nam', N'Việt Nam', N'813181846360');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user09', N'Kyle Tucker', N'sandra89@gmail.com', N'001-286-248-4830x2233', N'Nam', N'Việt Nam', N'202946055043');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user10', N'Robin Stanley', N'xhanson@davis.com', N'(806)561-5416x27573', N'Nam', N'Việt Nam', N'547390899554');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user11', N'Michelle Gutierrez', N'joseph84@brown-dunn.net', N'111-750-0394', N'Nữ', N'Việt Nam', N'333995652670');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user12', N'Richard Taylor', N'murphyjohn@diaz.net', N'001-961-878-8878x20604', N'Nữ', N'Việt Nam', N'168300472052');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user13', N'Blake Grant', N'beckyosborne@williams.org', N'+1-828-781-2272x27428', N'Nam', N'Việt Nam', N'101627677155');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user14', N'Krista Davidson', N'michaelsanders@atkinson.com', N'776.173.3783x510', N'Nữ', N'Việt Nam', N'962361016774');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user15', N'Michael Reese', N'lisa50@clark.info', N'627.819.6667x9509', N'Nữ', N'Việt Nam', N'565081604082');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user16', N'Joshua Fischer', N'bellgabriela@gmail.com', N'266.598.8608x985', N'Nữ', N'Việt Nam', N'306823277527');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user17', N'Sheri Lee', N'bethanycobb@lee.info', N'001-384-860-1291x55116', N'Nữ', N'Việt Nam', N'735906343638');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user18', N'Patrick Murphy', N'sfrederick@jones.biz', N'194.871.3297x62878', N'Nam', N'Việt Nam', N'446809914267');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user19', N'Sarah Pena', N'johnsonashley@gmail.com', N'215-256-4727x20824', N'Nam', N'Việt Nam', N'740165486786');


INSERT INTO NguoiDung (TenDangNhap, HoTen, Email, SoDienThoai, GioiTinh, QuocTich, CCCD)
VALUES (N'user20', N'Brenda Perry', N'mariomcbride@gmail.com', N'344-189-0820x3304', N'Nữ', N'Việt Nam', N'683482989002');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (6, 2, 17, N'Phổ thông', N'Khứ hồi', N'Đã thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (20, 3, 8, N'Thương gia', N'Khứ hồi', N'Đã thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (19, 20, 2, N'Phổ thông', N'Một chiều', N'Chưa thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (9, 7, 11, N'Phổ thông', N'Một chiều', N'Chưa thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (5, 10, 15, N'Thương gia', N'Khứ hồi', N'Đã thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (15, 20, 19, N'Phổ thông', N'Khứ hồi', N'Đã thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (17, 9, 5, N'Thương gia', N'Khứ hồi', N'Đã thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (12, 10, 6, N'Thương gia', N'Một chiều', N'Đã thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (18, 10, 4, N'Phổ thông', N'Một chiều', N'Đã thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (4, 18, 5, N'Thương gia', N'Một chiều', N'Đã thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (11, 7, 9, N'Thương gia', N'Một chiều', N'Đã thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (3, 14, 9, N'Phổ thông', N'Khứ hồi', N'Chưa thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (5, 9, 6, N'Thương gia', N'Một chiều', N'Đã thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (4, 3, 5, N'Phổ thông', N'Một chiều', N'Đã thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (14, 5, 2, N'Thương gia', N'Một chiều', N'Đã thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (12, 7, 8, N'Phổ thông', N'Một chiều', N'Chưa thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (20, 5, 8, N'Phổ thông', N'Khứ hồi', N'Chưa thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (1, 6, 11, N'Thương gia', N'Khứ hồi', N'Chưa thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (6, 4, 13, N'Phổ thông', N'Một chiều', N'Đã thanh toán');


INSERT INTO VeMayBay (IDNguoiDung, IDChuyenBay, IDGhe, HangGhe, LoaiVe, TrangThaiVe)
VALUES (7, 15, 12, N'Thương gia', N'Khứ hồi', N'Đã thanh toán');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (1, 2384189, N'Momo', '2025-05-26 15:11:17', N'Chờ xử lý');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (11, 1584272, N'Momo', '2025-07-07 09:35:45', N'Chờ xử lý');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (12, 2345284, N'Thẻ tín dụng', '2025-01-25 11:05:59', N'Chờ xử lý');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (18, 1694471, N'Momo', '2025-03-29 12:59:37', N'Thành công');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (9, 1374483, N'Thẻ tín dụng', '2025-03-20 03:31:50', N'Chờ xử lý');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (2, 1227336, N'Thẻ tín dụng', '2025-01-02 05:13:42', N'Chờ xử lý');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (12, 2527869, N'VNPay', '2025-06-24 13:11:09', N'Chờ xử lý');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (20, 2072531, N'Momo', '2025-04-17 10:13:53', N'Chờ xử lý');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (19, 1398624, N'VNPay', '2025-06-17 08:21:42', N'Thành công');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (14, 1003546, N'Thẻ tín dụng', '2025-04-24 14:38:06', N'Thất bại');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (7, 1763827, N'VNPay', '2025-06-16 06:19:41', N'Thành công');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (11, 2306851, N'VNPay', '2025-02-13 19:45:05', N'Thất bại');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (4, 2509434, N'VNPay', '2025-05-23 09:42:19', N'Thất bại');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (10, 2398575, N'VNPay', '2025-01-18 01:54:49', N'Chờ xử lý');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (13, 2462152, N'VNPay', '2025-05-03 15:28:49', N'Thất bại');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (5, 1402317, N'VNPay', '2025-05-30 11:08:02', N'Thất bại');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (13, 2420438, N'Thẻ tín dụng', '2025-05-17 09:33:43', N'Thành công');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (20, 2193500, N'VNPay', '2025-03-19 14:17:06', N'Chờ xử lý');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (18, 2748448, N'Momo', '2025-05-13 17:04:56', N'Chờ xử lý');


INSERT INTO ThanhToan (IDVe, SoTien, PhuongThuc, ThoiGianGiaoDich, TrangThaiThanhToan)
VALUES (10, 1440785, N'VNPay', '2025-03-22 04:47:36', N'Thất bại');
