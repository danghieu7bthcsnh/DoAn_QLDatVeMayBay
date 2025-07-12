using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class refund : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YeuCauHoanTien_NguoiDung_IDNguoiDung",
                table: "YeuCauHoanTien");

            migrationBuilder.DropForeignKey(
                name: "FK_YeuCauHoanTien_VeMayBay_IDVe",
                table: "YeuCauHoanTien");

            migrationBuilder.AddForeignKey(
                name: "FK_YeuCauHoanTien_NguoiDung_IDNguoiDung",
                table: "YeuCauHoanTien",
                column: "IDNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "IDNguoiDung",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_YeuCauHoanTien_VeMayBay_IDVe",
                table: "YeuCauHoanTien",
                column: "IDVe",
                principalTable: "VeMayBay",
                principalColumn: "IDVe",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YeuCauHoanTien_NguoiDung_IDNguoiDung",
                table: "YeuCauHoanTien");

            migrationBuilder.DropForeignKey(
                name: "FK_YeuCauHoanTien_VeMayBay_IDVe",
                table: "YeuCauHoanTien");

            migrationBuilder.AddForeignKey(
                name: "FK_YeuCauHoanTien_NguoiDung_IDNguoiDung",
                table: "YeuCauHoanTien",
                column: "IDNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "IDNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_YeuCauHoanTien_VeMayBay_IDVe",
                table: "YeuCauHoanTien",
                column: "IDVe",
                principalTable: "VeMayBay",
                principalColumn: "IDVe",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
