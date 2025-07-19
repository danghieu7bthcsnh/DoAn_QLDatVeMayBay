using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTheThanhToanModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TheThanhToanId",
                table: "TheThanhToan",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TheThanhToan_TheThanhToanId",
                table: "TheThanhToan",
                column: "TheThanhToanId");

            migrationBuilder.AddForeignKey(
                name: "FK_TheThanhToan_TheThanhToan_TheThanhToanId",
                table: "TheThanhToan",
                column: "TheThanhToanId",
                principalTable: "TheThanhToan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TheThanhToan_TheThanhToan_TheThanhToanId",
                table: "TheThanhToan");

            migrationBuilder.DropIndex(
                name: "IX_TheThanhToan_TheThanhToanId",
                table: "TheThanhToan");

            migrationBuilder.DropColumn(
                name: "TheThanhToanId",
                table: "TheThanhToan");
        }
    }
}
