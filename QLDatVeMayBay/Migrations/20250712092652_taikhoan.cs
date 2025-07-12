using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class taikhoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IDNguoiDung",
                table: "ThanhToan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgaySinh",
                table: "NguoiDung",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "QueQuan",
                table: "NguoiDung",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LienKetThe",
                columns: table => new
                {
                    IdLienKet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDNguoiDung = table.Column<int>(type: "int", nullable: false),
                    LoaiThe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoThe = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NganHang = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LienKetThe", x => x.IdLienKet);
                    table.ForeignKey(
                        name: "FK_LienKetThe_NguoiDung_IDNguoiDung",
                        column: x => x.IDNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "IDNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YeuCauHoanTien",
                columns: table => new
                {
                    IDHoanTien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDNguoiDung = table.Column<int>(type: "int", nullable: false),
                    IDVe = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuCauHoanTien", x => x.IDHoanTien);
                    table.ForeignKey(
                        name: "FK_YeuCauHoanTien_NguoiDung_IDNguoiDung",
                        column: x => x.IDNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "IDNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YeuCauHoanTien_VeMayBay_IDVe",
                        column: x => x.IDVe,
                        principalTable: "VeMayBay",
                        principalColumn: "IDVe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_IDNguoiDung",
                table: "ThanhToan",
                column: "IDNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_LienKetThe_IDNguoiDung",
                table: "LienKetThe",
                column: "IDNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauHoanTien_IDNguoiDung",
                table: "YeuCauHoanTien",
                column: "IDNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauHoanTien_IDVe",
                table: "YeuCauHoanTien",
                column: "IDVe");

            migrationBuilder.AddForeignKey(
                name: "FK_ThanhToan_NguoiDung_IDNguoiDung",
                table: "ThanhToan",
                column: "IDNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "IDNguoiDung",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThanhToan_NguoiDung_IDNguoiDung",
                table: "ThanhToan");

            migrationBuilder.DropTable(
                name: "LienKetThe");

            migrationBuilder.DropTable(
                name: "YeuCauHoanTien");

            migrationBuilder.DropIndex(
                name: "IX_ThanhToan_IDNguoiDung",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "IDNguoiDung",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "NgaySinh",
                table: "NguoiDung");

            migrationBuilder.DropColumn(
                name: "QueQuan",
                table: "NguoiDung");
        }
    }
}
