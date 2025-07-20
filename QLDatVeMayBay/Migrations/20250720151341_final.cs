using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenNganHang",
                table: "TheThanhToan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenNganHang",
                table: "TheThanhToan");
        }
    }
}
