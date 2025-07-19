using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class AddTheThanhToan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TheThanhToan",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NguoiDungId = table.Column<int>(type: "int", nullable: false),
                    Loai = table.Column<int>(type: "int", nullable: false),
                    SoThe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HieuLuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CVV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenTrenThe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenVi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailLienKet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenHienThi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayLienKet = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_TheThanhToan_NguoiDungId",
                table: "TheThanhToan",
                column: "NguoiDungId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TheThanhToan");
        }
    }
}
