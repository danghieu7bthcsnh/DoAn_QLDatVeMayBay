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
            migrationBuilder.AddColumn<int>(
                name: "LoaiMayBayId1",
                table: "MayBay",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MayBay_LoaiMayBayId1",
                table: "MayBay",
                column: "LoaiMayBayId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MayBay_LoaiMayBay_LoaiMayBayId1",
                table: "MayBay",
                column: "LoaiMayBayId1",
                principalTable: "LoaiMayBay",
                principalColumn: "LoaiMayBayId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MayBay_LoaiMayBay_LoaiMayBayId1",
                table: "MayBay");

            migrationBuilder.DropIndex(
                name: "IX_MayBay_LoaiMayBayId1",
                table: "MayBay");

            migrationBuilder.DropColumn(
                name: "LoaiMayBayId1",
                table: "MayBay");
        }
    }
}
