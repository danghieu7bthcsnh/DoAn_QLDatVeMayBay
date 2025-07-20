using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThanhToanIDThanhToan",
                table: "VeMayBay",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HoanTien",
                columns: table => new
                {
                    IDHoanTien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDThanhToan = table.Column<int>(type: "int", nullable: false),
                    SoTienHoan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayHoanTien = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoanTien", x => x.IDHoanTien);
                    table.ForeignKey(
                        name: "FK_HoanTien_ThanhToan_IDThanhToan",
                        column: x => x.IDThanhToan,
                        principalTable: "ThanhToan",
                        principalColumn: "IDThanhToan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TheThanhToan",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NguoiDungId = table.Column<int>(type: "int", nullable: false),
                    Loai = table.Column<int>(type: "int", nullable: false),
                    SoThe = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HieuLuc = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CVV = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    TenTrenThe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TenVi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmailLienKet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenHienThi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayLienKet = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TheThanhToanId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheThanhToan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TheThanhToan_NguoiDung_NguoiDungId",
                        column: x => x.NguoiDungId,
                        principalTable: "NguoiDung",
                        principalColumn: "IDNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TheThanhToan_TheThanhToan_TheThanhToanId",
                        column: x => x.TheThanhToanId,
                        principalTable: "TheThanhToan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VeMayBay_ThanhToanIDThanhToan",
                table: "VeMayBay",
                column: "ThanhToanIDThanhToan");

            migrationBuilder.CreateIndex(
                name: "IX_HoanTien_IDThanhToan",
                table: "HoanTien",
                column: "IDThanhToan");

            migrationBuilder.CreateIndex(
                name: "IX_TheThanhToan_NguoiDungId",
                table: "TheThanhToan",
                column: "NguoiDungId");

            migrationBuilder.CreateIndex(
                name: "IX_TheThanhToan_TheThanhToanId",
                table: "TheThanhToan",
                column: "TheThanhToanId");

            migrationBuilder.AddForeignKey(
                name: "FK_VeMayBay_ThanhToan_ThanhToanIDThanhToan",
                table: "VeMayBay",
                column: "ThanhToanIDThanhToan",
                principalTable: "ThanhToan",
                principalColumn: "IDThanhToan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VeMayBay_ThanhToan_ThanhToanIDThanhToan",
                table: "VeMayBay");

            migrationBuilder.DropTable(
                name: "HoanTien");

            migrationBuilder.DropTable(
                name: "TheThanhToan");

            migrationBuilder.DropIndex(
                name: "IX_VeMayBay_ThanhToanIDThanhToan",
                table: "VeMayBay");

            migrationBuilder.DropColumn(
                name: "ThanhToanIDThanhToan",
                table: "VeMayBay");
        }
    }
}
