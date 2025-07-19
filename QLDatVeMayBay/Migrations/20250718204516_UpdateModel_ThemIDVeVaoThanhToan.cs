using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel_ThemIDVeVaoThanhToan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VeMayBay_ThanhToan_IDThanhToan",
                table: "VeMayBay");

            migrationBuilder.RenameColumn(
                name: "IDThanhToan",
                table: "VeMayBay",
                newName: "ThanhToanIDThanhToan");

            migrationBuilder.RenameIndex(
                name: "IX_VeMayBay_IDThanhToan",
                table: "VeMayBay",
                newName: "IX_VeMayBay_ThanhToanIDThanhToan");

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

            migrationBuilder.RenameColumn(
                name: "ThanhToanIDThanhToan",
                table: "VeMayBay",
                newName: "IDThanhToan");

            migrationBuilder.RenameIndex(
                name: "IX_VeMayBay_ThanhToanIDThanhToan",
                table: "VeMayBay",
                newName: "IX_VeMayBay_IDThanhToan");

            migrationBuilder.AddForeignKey(
                name: "FK_VeMayBay_ThanhToan_IDThanhToan",
                table: "VeMayBay",
                column: "IDThanhToan",
                principalTable: "ThanhToan",
                principalColumn: "IDThanhToan");
        }
    }
}
