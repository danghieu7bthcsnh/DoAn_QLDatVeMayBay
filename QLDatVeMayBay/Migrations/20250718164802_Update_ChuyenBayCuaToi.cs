using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class Update_ChuyenBayCuaToi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThanhToanIDThanhToan",
                table: "VeMayBay",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VeMayBay_ThanhToanIDThanhToan",
                table: "VeMayBay",
                column: "ThanhToanIDThanhToan");

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

            migrationBuilder.DropIndex(
                name: "IX_VeMayBay_ThanhToanIDThanhToan",
                table: "VeMayBay");

            migrationBuilder.DropColumn(
                name: "ThanhToanIDThanhToan",
                table: "VeMayBay");
        }
    }
}
