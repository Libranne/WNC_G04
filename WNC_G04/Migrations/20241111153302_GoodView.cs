using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WNC_G04.Migrations
{
    /// <inheritdoc />
    public partial class GoodView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChuDe",
                columns: table => new
                {
                    MaChuDe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenChuDe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuDe", x => x.MaChuDe);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNguoiDung = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ConfirmMatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnhDaiDien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TieuSu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "BaiViet",
                columns: table => new
                {
                    MaBaiViet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnhBaiViet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaChuDe = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    luotthich = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaiViet", x => x.MaBaiViet);
                    table.ForeignKey(
                        name: "FK_BaiViet_ChuDe_MaChuDe",
                        column: x => x.MaChuDe,
                        principalTable: "ChuDe",
                        principalColumn: "MaChuDe");
                    table.ForeignKey(
                        name: "FK_BaiViet_NguoiDung_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "TheoDoi",
                columns: table => new
                {
                    MaTheoDoi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiTheoDoi = table.Column<int>(type: "int", nullable: true),
                    MaNguoiDuocTheoDoi = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheoDoi", x => x.MaTheoDoi);
                    table.ForeignKey(
                        name: "FK_TheoDoi_NguoiDung_MaNguoiDuocTheoDoi",
                        column: x => x.MaNguoiDuocTheoDoi,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK_TheoDoi_NguoiDung_MaNguoiTheoDoi",
                        column: x => x.MaNguoiTheoDoi,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "BinhLuan",
                columns: table => new
                {
                    MaBinhLuan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaBaiViet = table.Column<int>(type: "int", nullable: true),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinhLuan", x => x.MaBinhLuan);
                    table.ForeignKey(
                        name: "FK_BinhLuan_BaiViet_MaBaiViet",
                        column: x => x.MaBaiViet,
                        principalTable: "BaiViet",
                        principalColumn: "MaBaiViet");
                    table.ForeignKey(
                        name: "FK_BinhLuan_NguoiDung_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "Thich",
                columns: table => new
                {
                    MaThich = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaBaiViet = table.Column<int>(type: "int", nullable: true),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thich", x => x.MaThich);
                    table.ForeignKey(
                        name: "FK_Thich_BaiViet_MaBaiViet",
                        column: x => x.MaBaiViet,
                        principalTable: "BaiViet",
                        principalColumn: "MaBaiViet");
                    table.ForeignKey(
                        name: "FK_Thich_NguoiDung_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "ThongBao",
                columns: table => new
                {
                    MaThongBao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    LoaiThongBao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaBaiViet = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBao", x => x.MaThongBao);
                    table.ForeignKey(
                        name: "FK_ThongBao_BaiViet_MaBaiViet",
                        column: x => x.MaBaiViet,
                        principalTable: "BaiViet",
                        principalColumn: "MaBaiViet");
                    table.ForeignKey(
                        name: "FK_ThongBao_NguoiDung_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaiViet_MaChuDe",
                table: "BaiViet",
                column: "MaChuDe");

            migrationBuilder.CreateIndex(
                name: "IX_BaiViet_MaNguoiDung",
                table: "BaiViet",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuan_MaBaiViet",
                table: "BinhLuan",
                column: "MaBaiViet");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuan_MaNguoiDung",
                table: "BinhLuan",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_TheoDoi_MaNguoiDuocTheoDoi",
                table: "TheoDoi",
                column: "MaNguoiDuocTheoDoi");

            migrationBuilder.CreateIndex(
                name: "IX_TheoDoi_MaNguoiTheoDoi_MaNguoiDuocTheoDoi",
                table: "TheoDoi",
                columns: new[] { "MaNguoiTheoDoi", "MaNguoiDuocTheoDoi" },
                unique: true,
                filter: "[MaNguoiTheoDoi] IS NOT NULL AND [MaNguoiDuocTheoDoi] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Thich_MaBaiViet",
                table: "Thich",
                column: "MaBaiViet");

            migrationBuilder.CreateIndex(
                name: "IX_Thich_MaNguoiDung",
                table: "Thich",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBao_MaBaiViet",
                table: "ThongBao",
                column: "MaBaiViet");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBao_MaNguoiDung",
                table: "ThongBao",
                column: "MaNguoiDung");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BinhLuan");

            migrationBuilder.DropTable(
                name: "TheoDoi");

            migrationBuilder.DropTable(
                name: "Thich");

            migrationBuilder.DropTable(
                name: "ThongBao");

            migrationBuilder.DropTable(
                name: "BaiViet");

            migrationBuilder.DropTable(
                name: "ChuDe");

            migrationBuilder.DropTable(
                name: "NguoiDung");
        }
    }
}
