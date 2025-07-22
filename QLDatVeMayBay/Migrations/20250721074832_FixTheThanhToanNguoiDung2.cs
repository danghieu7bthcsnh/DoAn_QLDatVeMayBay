using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class FixTheThanhToanNguoiDung2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IDNguoiDung1",
                table: "TheThanhToan",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IDNguoiDung1",
                table: "TheThanhToan");
        }
    }
}
