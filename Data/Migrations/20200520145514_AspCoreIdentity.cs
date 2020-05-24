using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AspCoreIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    ProviderKey = table.Column<string>(nullable: true),
                    ProviderDisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokens", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "GiangVien",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false),
                    Ho = table.Column<string>(maxLength: 30, nullable: true),
                    Ten = table.Column<string>(maxLength: 100, nullable: true),
                    GioiTinh = table.Column<int>(nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "date", nullable: true),
                    DiaChi = table.Column<string>(nullable: true),
                    SDT = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    Email = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiangVien", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HoiDong",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayLap = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoiDong", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NamHoc",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HocKy = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    Nam = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NamHoc", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "QuanLy",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false),
                    Ho = table.Column<string>(maxLength: 30, nullable: true),
                    Ten = table.Column<string>(maxLength: 100, nullable: true),
                    GioiTinh = table.Column<int>(nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "date", nullable: true),
                    DiaChi = table.Column<string>(nullable: true),
                    SDT = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    Email = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuanLy", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    MoTa = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SinhVien",
                columns: table => new
                {
                    MSSV = table.Column<long>(nullable: false),
                    Ho = table.Column<string>(maxLength: 30, nullable: true),
                    Ten = table.Column<string>(maxLength: 100, nullable: true),
                    GioiTinh = table.Column<int>(nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "date", nullable: true),
                    DiaChi = table.Column<string>(nullable: true),
                    SDT = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    Email = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SinhVien__6CB3B7F96B8F18DF", x => x.MSSV);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Ho = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KenhThaoLuan",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDGiangVien = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KenhThaoLuan", x => x.ID);
                    table.ForeignKey(
                        name: "FK__KenhThaoL__IDGia__60A75C0F",
                        column: x => x.IDGiangVien,
                        principalTable: "GiangVien",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Nhom",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhom = table.Column<string>(maxLength: 50, nullable: true),
                    IDNamHoc = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nhom", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Nhom__IDNamHoc__3A81B327",
                        column: x => x.IDNamHoc,
                        principalTable: "NamHoc",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoNhiem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDGiangVien = table.Column<long>(nullable: true),
                    IDQuanLy = table.Column<long>(nullable: true),
                    IDHoiDong = table.Column<int>(nullable: true),
                    VaiTro = table.Column<int>(nullable: true),
                    NgayBoNhiem = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoNhiem", x => x.ID);
                    table.ForeignKey(
                        name: "FK__BoNhiem__IDGiang__49C3F6B7",
                        column: x => x.IDGiangVien,
                        principalTable: "GiangVien",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__BoNhiem__IDHoiDo__4BAC3F29",
                        column: x => x.IDHoiDong,
                        principalTable: "HoiDong",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__BoNhiem__IDQuanL__4AB81AF0",
                        column: x => x.IDQuanLy,
                        principalTable: "QuanLy",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MoDot",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDNamHoc = table.Column<int>(nullable: true),
                    IDQuanLy = table.Column<long>(nullable: true),
                    ThoiGianBD = table.Column<DateTime>(type: "datetime", nullable: true),
                    ThoiGianKT = table.Column<DateTime>(type: "datetime", nullable: true),
                    Loai = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoDot", x => x.ID);
                    table.ForeignKey(
                        name: "FK__MoDot__IDNamHoc__52593CB8",
                        column: x => x.IDNamHoc,
                        principalTable: "NamHoc",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__MoDot__IDQuanLy__534D60F1",
                        column: x => x.IDQuanLy,
                        principalTable: "QuanLy",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeTaiNghienCuu",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false),
                    TenDeTai = table.Column<string>(nullable: true),
                    MoTa = table.Column<string>(nullable: true),
                    TepDinhKem = table.Column<string>(nullable: true),
                    IDGiangVien = table.Column<long>(nullable: true),
                    IDNhom = table.Column<int>(nullable: true),
                    NgayLap = table.Column<DateTime>(type: "datetime", nullable: true),
                    Loai = table.Column<bool>(nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeTaiNghienCuu", x => x.ID);
                    table.ForeignKey(
                        name: "FK__DeTaiNghi__IDGia__4E88ABD4",
                        column: x => x.IDGiangVien,
                        principalTable: "GiangVien",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__DeTaiNghi__IDNho__4F7CD00D",
                        column: x => x.IDNhom,
                        principalTable: "Nhom",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Nhom_SinhVien",
                columns: table => new
                {
                    IDNhom = table.Column<int>(nullable: false),
                    IDSinhVien = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Nhom_Sin__CD149E43F71CE773", x => new { x.IDNhom, x.IDSinhVien });
                    table.ForeignKey(
                        name: "FK__Nhom_Sinh__Statu__3F466844",
                        column: x => x.IDNhom,
                        principalTable: "Nhom",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Nhom_Sinh__IDSin__403A8C7D",
                        column: x => x.IDSinhVien,
                        principalTable: "SinhVien",
                        principalColumn: "MSSV",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaoCaoTienDo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDDeTai = table.Column<long>(nullable: true),
                    NoiDung = table.Column<string>(nullable: true),
                    TepDinhKem = table.Column<string>(nullable: true),
                    TienDo = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    DanhGia = table.Column<string>(nullable: true),
                    NgayNop = table.Column<DateTime>(type: "date", nullable: true),
                    HanNop = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaoCaoTienDo", x => x.ID);
                    table.ForeignKey(
                        name: "FK__BaoCaoTie__IDDeT__5DCAEF64",
                        column: x => x.IDDeTai,
                        principalTable: "DeTaiNghienCuu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CTKenhThaoLuan",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDKenhThaoLuan = table.Column<int>(nullable: true),
                    IDDeTai = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTKenhThaoLuan", x => x.ID);
                    table.ForeignKey(
                        name: "FK__CTKenhTha__IDDeT__6477ECF3",
                        column: x => x.IDDeTai,
                        principalTable: "DeTaiNghienCuu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__CTKenhTha__IDKen__6383C8BA",
                        column: x => x.IDKenhThaoLuan,
                        principalTable: "KenhThaoLuan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "XetDuyetVaDanhGia",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDDeTai = table.Column<long>(nullable: true),
                    IDHoiDong = table.Column<int>(nullable: true),
                    IDMoDot = table.Column<int>(nullable: true),
                    NoiDung = table.Column<string>(nullable: true),
                    TepDinhKem = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XetDuyetVaDanhGia", x => x.ID);
                    table.ForeignKey(
                        name: "FK__XetDuyetV__IDDeT__5629CD9C",
                        column: x => x.IDDeTai,
                        principalTable: "DeTaiNghienCuu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__XetDuyetV__IDHoi__571DF1D5",
                        column: x => x.IDHoiDong,
                        principalTable: "HoiDong",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__XetDuyetV__IDMoD__5812160E",
                        column: x => x.IDMoDot,
                        principalTable: "MoDot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaiPost",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDNguoiTao = table.Column<int>(nullable: false),
                    IDCTKenhThaoLuan = table.Column<int>(nullable: true),
                    NgayPost = table.Column<DateTime>(type: "datetime", nullable: true),
                    TieuDe = table.Column<string>(maxLength: 150, nullable: true),
                    NoiDung = table.Column<DateTime>(type: "date", nullable: true),
                    Loai = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaiPost", x => x.ID);
                    table.ForeignKey(
                        name: "FK__BaiPost__IDCTKen__6754599E",
                        column: x => x.IDCTKenhThaoLuan,
                        principalTable: "CTKenhThaoLuan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CTXetDuyetVaDanhGia",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDXetDuyet = table.Column<int>(nullable: true),
                    IDNguoiTao = table.Column<int>(nullable: false),
                    Diem = table.Column<double>(nullable: true),
                    NhanXet = table.Column<string>(nullable: true),
                    CauHoi = table.Column<string>(nullable: true),
                    CauTraLoi = table.Column<string>(nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTXetDuyetVaDanhGia", x => x.ID);
                    table.ForeignKey(
                        name: "FK__CTXetDuye__IDXet__5AEE82B9",
                        column: x => x.IDXetDuyet,
                        principalTable: "XetDuyetVaDanhGia",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDNguoiTao = table.Column<int>(nullable: false),
                    IDBaiPost = table.Column<int>(nullable: true),
                    NoiDungComment = table.Column<string>(nullable: true),
                    AnhDinhKem = table.Column<string>(nullable: true),
                    NgayPost = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Comments__IDBaiP__6A30C649",
                        column: x => x.IDBaiPost,
                        principalTable: "BaiPost",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaiPost_IDCTKenhThaoLuan",
                table: "BaiPost",
                column: "IDCTKenhThaoLuan");

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoTienDo_IDDeTai",
                table: "BaoCaoTienDo",
                column: "IDDeTai");

            migrationBuilder.CreateIndex(
                name: "IX_BoNhiem_IDHoiDong",
                table: "BoNhiem",
                column: "IDHoiDong");

            migrationBuilder.CreateIndex(
                name: "IX_BoNhiem_IDQuanLy",
                table: "BoNhiem",
                column: "IDQuanLy");

            migrationBuilder.CreateIndex(
                name: "UNI_BoNhiem",
                table: "BoNhiem",
                columns: new[] { "IDGiangVien", "IDQuanLy", "IDHoiDong" },
                unique: true,
                filter: "[IDGiangVien] IS NOT NULL AND [IDQuanLy] IS NOT NULL AND [IDHoiDong] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IDBaiPost",
                table: "Comments",
                column: "IDBaiPost");

            migrationBuilder.CreateIndex(
                name: "IX_CTKenhThaoLuan_IDDeTai",
                table: "CTKenhThaoLuan",
                column: "IDDeTai");

            migrationBuilder.CreateIndex(
                name: "IX_CTKenhThaoLuan_IDKenhThaoLuan",
                table: "CTKenhThaoLuan",
                column: "IDKenhThaoLuan");

            migrationBuilder.CreateIndex(
                name: "IX_CTXetDuyetVaDanhGia_IDXetDuyet",
                table: "CTXetDuyetVaDanhGia",
                column: "IDXetDuyet");

            migrationBuilder.CreateIndex(
                name: "IX_DeTaiNghienCuu_IDGiangVien",
                table: "DeTaiNghienCuu",
                column: "IDGiangVien");

            migrationBuilder.CreateIndex(
                name: "IX_DeTaiNghienCuu_IDNhom",
                table: "DeTaiNghienCuu",
                column: "IDNhom");

            migrationBuilder.CreateIndex(
                name: "IX_KenhThaoLuan_IDGiangVien",
                table: "KenhThaoLuan",
                column: "IDGiangVien");

            migrationBuilder.CreateIndex(
                name: "IX_MoDot_IDNamHoc",
                table: "MoDot",
                column: "IDNamHoc");

            migrationBuilder.CreateIndex(
                name: "IX_MoDot_IDQuanLy",
                table: "MoDot",
                column: "IDQuanLy");

            migrationBuilder.CreateIndex(
                name: "UNI_NamHoc",
                table: "NamHoc",
                columns: new[] { "HocKy", "Nam" },
                unique: true,
                filter: "[HocKy] IS NOT NULL AND [Nam] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Nhom_IDNamHoc",
                table: "Nhom",
                column: "IDNamHoc");

            migrationBuilder.CreateIndex(
                name: "IX_Nhom_SinhVien_IDSinhVien",
                table: "Nhom_SinhVien",
                column: "IDSinhVien");

            migrationBuilder.CreateIndex(
                name: "IX_XetDuyetVaDanhGia_IDDeTai",
                table: "XetDuyetVaDanhGia",
                column: "IDDeTai");

            migrationBuilder.CreateIndex(
                name: "IX_XetDuyetVaDanhGia_IDHoiDong",
                table: "XetDuyetVaDanhGia",
                column: "IDHoiDong");

            migrationBuilder.CreateIndex(
                name: "IX_XetDuyetVaDanhGia_IDMoDot",
                table: "XetDuyetVaDanhGia",
                column: "IDMoDot");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropTable(
                name: "AppUserClaims");

            migrationBuilder.DropTable(
                name: "AppUserLogins");

            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "AppUserTokens");

            migrationBuilder.DropTable(
                name: "BaoCaoTienDo");

            migrationBuilder.DropTable(
                name: "BoNhiem");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "CTXetDuyetVaDanhGia");

            migrationBuilder.DropTable(
                name: "Nhom_SinhVien");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "BaiPost");

            migrationBuilder.DropTable(
                name: "XetDuyetVaDanhGia");

            migrationBuilder.DropTable(
                name: "SinhVien");

            migrationBuilder.DropTable(
                name: "CTKenhThaoLuan");

            migrationBuilder.DropTable(
                name: "HoiDong");

            migrationBuilder.DropTable(
                name: "MoDot");

            migrationBuilder.DropTable(
                name: "DeTaiNghienCuu");

            migrationBuilder.DropTable(
                name: "KenhThaoLuan");

            migrationBuilder.DropTable(
                name: "QuanLy");

            migrationBuilder.DropTable(
                name: "Nhom");

            migrationBuilder.DropTable(
                name: "GiangVien");

            migrationBuilder.DropTable(
                name: "NamHoc");
        }
    }
}
