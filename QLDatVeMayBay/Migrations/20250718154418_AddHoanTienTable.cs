using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class AddHoanTienTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HoanTien",
                columns: table => new
                {
                    IDHoanTien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDThanhToan = table.Column<int>(type: "int", nullable: false),
                    SoTienHoan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayHoanTien = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoanTien", x => x.IDHoanTien);
                    table.ForeignKey(
                        name: "FK_HoanTien_ThanhToan_IDThanhToan",
                        column: x => x.IDThanhToan,
                        principalTable: "ThanhToan",
                        principalColumn: "IDThanhToan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoanTien_IDThanhToan",
                table: "HoanTien",
                column: "IDThanhToan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoanTien");
        }
    }
}
