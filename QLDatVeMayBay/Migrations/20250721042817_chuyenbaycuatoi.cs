using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class chuyenbaycuatoi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IDTaiKhoan",
                table: "VeMayBay",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TaiKhoanTenDangNhap",
                table: "VeMayBay",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VeMayBay_TaiKhoanTenDangNhap",
                table: "VeMayBay",
                column: "TaiKhoanTenDangNhap");

            migrationBuilder.AddForeignKey(
                name: "FK_VeMayBay_TaiKhoan_TaiKhoanTenDangNhap",
                table: "VeMayBay",
                column: "TaiKhoanTenDangNhap",
                principalTable: "TaiKhoan",
                principalColumn: "TenDangNhap");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VeMayBay_TaiKhoan_TaiKhoanTenDangNhap",
                table: "VeMayBay");

            migrationBuilder.DropIndex(
                name: "IX_VeMayBay_TaiKhoanTenDangNhap",
                table: "VeMayBay");

            migrationBuilder.DropColumn(
                name: "IDTaiKhoan",
                table: "VeMayBay");

            migrationBuilder.DropColumn(
                name: "TaiKhoanTenDangNhap",
                table: "VeMayBay");
        }
    }
}
