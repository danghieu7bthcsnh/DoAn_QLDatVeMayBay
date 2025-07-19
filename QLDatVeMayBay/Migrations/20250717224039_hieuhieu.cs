using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class hieuhieu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoaiMayBay",
                columns: table => new
                {
                    LoaiMayBayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TongSoGhe = table.Column<int>(type: "int", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiMayBay", x => x.LoaiMayBayId);
                });

            migrationBuilder.CreateTable(
                name: "MaXacNhan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiGianHetHan = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaXacNhan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SanBay",
                columns: table => new
                {
                    IDSanBay = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSanBay = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiaDiem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanBay", x => x.IDSanBay);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TrangThaiTK = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.TenDangNhap);
                });

            migrationBuilder.CreateTable(
                name: "MayBay",
                columns: table => new
                {
                    IDMayBay = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenHangHK = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LoaiMayBayId = table.Column<int>(type: "int", nullable: false),
                    LoaiMayBayId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MayBay", x => x.IDMayBay);
                    table.ForeignKey(
                        name: "FK_MayBay_LoaiMayBay_LoaiMayBayId",
                        column: x => x.LoaiMayBayId,
                        principalTable: "LoaiMayBay",
                        principalColumn: "LoaiMayBayId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MayBay_LoaiMayBay_LoaiMayBayId1",
                        column: x => x.LoaiMayBayId1,
                        principalTable: "LoaiMayBay",
                        principalColumn: "LoaiMayBayId");
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    IDNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    QuocTich = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CCCD = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.IDNguoiDung);
                    table.ForeignKey(
                        name: "FK_NguoiDung_TaiKhoan_TenDangNhap",
                        column: x => x.TenDangNhap,
                        principalTable: "TaiKhoan",
                        principalColumn: "TenDangNhap",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChuyenBay",
                columns: table => new
                {
                    IDChuyenBay = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDMayBay = table.Column<int>(type: "int", nullable: false),
                    SanBayDi = table.Column<int>(type: "int", nullable: false),
                    SanBayDen = table.Column<int>(type: "int", nullable: false),
                    GioCatCanh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioHaCanh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GiaVe = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuyenBay", x => x.IDChuyenBay);
                    table.ForeignKey(
                        name: "FK_ChuyenBay_MayBay_IDMayBay",
                        column: x => x.IDMayBay,
                        principalTable: "MayBay",
                        principalColumn: "IDMayBay",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChuyenBay_SanBay_SanBayDen",
                        column: x => x.SanBayDen,
                        principalTable: "SanBay",
                        principalColumn: "IDSanBay",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChuyenBay_SanBay_SanBayDi",
                        column: x => x.SanBayDi,
                        principalTable: "SanBay",
                        principalColumn: "IDSanBay",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GheNgoi",
                columns: table => new
                {
                    IDGhe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDChuyenBay = table.Column<int>(type: "int", nullable: false),
                    HangGhe = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GheNgoi", x => x.IDGhe);
                    table.ForeignKey(
                        name: "FK_GheNgoi_ChuyenBay_IDChuyenBay",
                        column: x => x.IDChuyenBay,
                        principalTable: "ChuyenBay",
                        principalColumn: "IDChuyenBay",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VeMayBay",
                columns: table => new
                {
                    IDVe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDNguoiDung = table.Column<int>(type: "int", nullable: false),
                    IDChuyenBay = table.Column<int>(type: "int", nullable: false),
                    IDGhe = table.Column<int>(type: "int", nullable: false),
                    ThoiGianDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiVe = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HangGhe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoaiVe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NguoiDungIDNguoiDung = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VeMayBay", x => x.IDVe);
                    table.ForeignKey(
                        name: "FK_VeMayBay_ChuyenBay_IDChuyenBay",
                        column: x => x.IDChuyenBay,
                        principalTable: "ChuyenBay",
                        principalColumn: "IDChuyenBay",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VeMayBay_GheNgoi_IDGhe",
                        column: x => x.IDGhe,
                        principalTable: "GheNgoi",
                        principalColumn: "IDGhe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VeMayBay_NguoiDung_IDNguoiDung",
                        column: x => x.IDNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "IDNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VeMayBay_NguoiDung_NguoiDungIDNguoiDung",
                        column: x => x.NguoiDungIDNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "IDNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "ThanhToan",
                columns: table => new
                {
                    IDThanhToan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDVe = table.Column<int>(type: "int", nullable: false),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhuongThuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ThoiGianGiaoDich = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiThanhToan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToan", x => x.IDThanhToan);
                    table.ForeignKey(
                        name: "FK_ThanhToan_VeMayBay_IDVe",
                        column: x => x.IDVe,
                        principalTable: "VeMayBay",
                        principalColumn: "IDVe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenBay_IDMayBay",
                table: "ChuyenBay",
                column: "IDMayBay");

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenBay_SanBayDen",
                table: "ChuyenBay",
                column: "SanBayDen");

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenBay_SanBayDi",
                table: "ChuyenBay",
                column: "SanBayDi");

            migrationBuilder.CreateIndex(
                name: "IX_GheNgoi_IDChuyenBay",
                table: "GheNgoi",
                column: "IDChuyenBay");

            migrationBuilder.CreateIndex(
                name: "IX_MayBay_LoaiMayBayId",
                table: "MayBay",
                column: "LoaiMayBayId");

            migrationBuilder.CreateIndex(
                name: "IX_MayBay_LoaiMayBayId1",
                table: "MayBay",
                column: "LoaiMayBayId1");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_TenDangNhap",
                table: "NguoiDung",
                column: "TenDangNhap",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_IDVe",
                table: "ThanhToan",
                column: "IDVe");

            migrationBuilder.CreateIndex(
                name: "IX_VeMayBay_IDChuyenBay",
                table: "VeMayBay",
                column: "IDChuyenBay");

            migrationBuilder.CreateIndex(
                name: "IX_VeMayBay_IDGhe",
                table: "VeMayBay",
                column: "IDGhe");

            migrationBuilder.CreateIndex(
                name: "IX_VeMayBay_IDNguoiDung",
                table: "VeMayBay",
                column: "IDNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_VeMayBay_NguoiDungIDNguoiDung",
                table: "VeMayBay",
                column: "NguoiDungIDNguoiDung");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaXacNhan");

            migrationBuilder.DropTable(
                name: "ThanhToan");

            migrationBuilder.DropTable(
                name: "VeMayBay");

            migrationBuilder.DropTable(
                name: "GheNgoi");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "ChuyenBay");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "MayBay");

            migrationBuilder.DropTable(
                name: "SanBay");

            migrationBuilder.DropTable(
                name: "LoaiMayBay");
        }
    }
}
