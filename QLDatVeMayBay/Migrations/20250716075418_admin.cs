using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class admin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NguoiDungIDNguoiDung",
                table: "VeMayBay",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTao",
                table: "TaiKhoan",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VeMayBay_IDGhe",
                table: "VeMayBay",
                column: "IDGhe");

            migrationBuilder.CreateIndex(
                name: "IX_VeMayBay_NguoiDungIDNguoiDung",
                table: "VeMayBay",
                column: "NguoiDungIDNguoiDung");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_VeMayBay_GheNgoi_IDGhe",
            //    table: "VeMayBay",
            //    column: "IDGhe",
            //    principalTable: "GheNgoi",
            //    principalColumn: "IDGhe",
            //    onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VeMayBay_NguoiDung_NguoiDungIDNguoiDung",
                table: "VeMayBay",
                column: "NguoiDungIDNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "IDNguoiDung");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VeMayBay_GheNgoi_IDGhe",
                table: "VeMayBay");

            migrationBuilder.DropForeignKey(
                name: "FK_VeMayBay_NguoiDung_NguoiDungIDNguoiDung",
                table: "VeMayBay");

            migrationBuilder.DropIndex(
                name: "IX_VeMayBay_IDGhe",
                table: "VeMayBay");

            migrationBuilder.DropIndex(
                name: "IX_VeMayBay_NguoiDungIDNguoiDung",
                table: "VeMayBay");

            migrationBuilder.DropColumn(
                name: "NguoiDungIDNguoiDung",
                table: "VeMayBay");

            migrationBuilder.DropColumn(
                name: "NgayTao",
                table: "TaiKhoan");
        }
    }
}
