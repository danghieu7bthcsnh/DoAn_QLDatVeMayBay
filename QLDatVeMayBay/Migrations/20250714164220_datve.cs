using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class datve : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Xóa foreign key FK từ MayBay → LoaiMayBay
            migrationBuilder.DropForeignKey(
                name: "FK_MayBay_LoaiMayBay_LoaiMayBayId",
                table: "MayBay");

            // 2. Đổi tên cột cũ trong bảng LoaiMayBay để giữ dữ liệu
            migrationBuilder.RenameColumn(
                name: "LoaiMayBayId",
                table: "LoaiMayBay",
                newName: "LoaiMayBayId_Old");

            // 3. Thêm cột mới kiểu int + identity
            migrationBuilder.AddColumn<int>(
                name: "LoaiMayBayId",
                table: "LoaiMayBay",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // 4. Gán lại Primary Key mới
            migrationBuilder.DropPrimaryKey(
                name: "PK_LoaiMayBay",
                table: "LoaiMayBay");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiMayBay",
                table: "LoaiMayBay",
                column: "LoaiMayBayId");

            // 5. Sửa cột LoaiMayBayId ở bảng MayBay về kiểu int
            migrationBuilder.AlterColumn<int>(
                name: "LoaiMayBayId",
                table: "MayBay",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            // 6. Thêm lại foreign key mới từ MayBay → LoaiMayBay
            migrationBuilder.AddForeignKey(
                name: "FK_MayBay_LoaiMayBay_LoaiMayBayId",
                table: "MayBay",
                column: "LoaiMayBayId",
                principalTable: "LoaiMayBay",
                principalColumn: "LoaiMayBayId",
                onDelete: ReferentialAction.Cascade);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MayBay_LoaiMayBay_LoaiMayBayId",
                table: "MayBay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoaiMayBay",
                table: "LoaiMayBay");

            migrationBuilder.DropColumn(
                name: "LoaiMayBayId",
                table: "LoaiMayBay");

            migrationBuilder.RenameColumn(
                name: "LoaiMayBayId_Old",
                table: "LoaiMayBay",
                newName: "LoaiMayBayId");

            migrationBuilder.AlterColumn<string>(
                name: "LoaiMayBayId",
                table: "MayBay",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiMayBay",
                table: "LoaiMayBay",
                column: "LoaiMayBayId");

            migrationBuilder.AddForeignKey(
                name: "FK_MayBay_LoaiMayBay_LoaiMayBayId",
                table: "MayBay",
                column: "LoaiMayBayId",
                principalTable: "LoaiMayBay",
                principalColumn: "LoaiMayBayId",
                onDelete: ReferentialAction.Cascade);
        }


    }
}
