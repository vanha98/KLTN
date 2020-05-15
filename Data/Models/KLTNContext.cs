using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.Models
{
    public partial class KLTNContext : DbContext
    {
        public KLTNContext()
        {
        }

        public KLTNContext(DbContextOptions<KLTNContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BaiPost> BaiPost { get; set; }
        public virtual DbSet<BaoCaoHangTuan> BaoCaoHangTuan { get; set; }
        public virtual DbSet<BoNhiem> BoNhiem { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<CtkenhThaoLuan> CtkenhThaoLuan { get; set; }
        public virtual DbSet<CtxetDuyetVaDanhGia> CtxetDuyetVaDanhGia { get; set; }
        public virtual DbSet<DeTaiNghienCuu> DeTaiNghienCuu { get; set; }
        public virtual DbSet<GiangVien> GiangVien { get; set; }
        public virtual DbSet<HoiDong> HoiDong { get; set; }
        public virtual DbSet<KenhThaoLuan> KenhThaoLuan { get; set; }
        public virtual DbSet<MoDot> MoDot { get; set; }
        public virtual DbSet<NamHoc> NamHoc { get; set; }
        public virtual DbSet<NhomSv> NhomSv { get; set; }
        public virtual DbSet<QuanLy> QuanLy { get; set; }
        public virtual DbSet<SinhVien> SinhVien { get; set; }
        public virtual DbSet<XetDuyetVaDanhGia> XetDuyetVaDanhGia { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-U7OPBBM;Database=KLTN;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaiPost>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdctkenhThaoLuan).HasColumnName("IDCTKenhThaoLuan");

                entity.Property(e => e.IdnguoiTao).HasColumnName("IDNguoiTao");

                entity.Property(e => e.NgayPost).HasColumnType("datetime");

                entity.Property(e => e.NoiDung).HasColumnType("date");

                entity.Property(e => e.TieuDe).HasMaxLength(150);

                entity.HasOne(d => d.IdctkenhThaoLuanNavigation)
                    .WithMany(p => p.BaiPost)
                    .HasForeignKey(d => d.IdctkenhThaoLuan)
                    .HasConstraintName("FK__BaiPost__IDCTKen__3D5E1FD2");
            });

            modelBuilder.Entity<BaoCaoHangTuan>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HanNop).HasColumnType("date");

                entity.Property(e => e.IddeTai).HasColumnName("IDDeTai");

                entity.Property(e => e.NgayNop).HasColumnType("date");

                entity.Property(e => e.TienDo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IddeTaiNavigation)
                    .WithMany(p => p.BaoCaoHangTuan)
                    .HasForeignKey(d => d.IddeTai)
                    .HasConstraintName("FK__BaoCaoHan__IDDeT__33D4B598");
            });

            modelBuilder.Entity<BoNhiem>(entity =>
            {
                entity.HasIndex(e => new { e.IdgiangVien, e.IdquanLy, e.IdhoiDong })
                    .HasName("UNI_BoNhiem")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdgiangVien).HasColumnName("IDGiangVien");

                entity.Property(e => e.IdhoiDong).HasColumnName("IDHoiDong");

                entity.Property(e => e.IdquanLy).HasColumnName("IDQuanLy");

                entity.Property(e => e.NgayBoNhiem).HasColumnType("datetime");

                entity.HasOne(d => d.IdgiangVienNavigation)
                    .WithMany(p => p.BoNhiem)
                    .HasForeignKey(d => d.IdgiangVien)
                    .HasConstraintName("FK__BoNhiem__IDGiang__1FCDBCEB");

                entity.HasOne(d => d.IdhoiDongNavigation)
                    .WithMany(p => p.BoNhiem)
                    .HasForeignKey(d => d.IdhoiDong)
                    .HasConstraintName("FK__BoNhiem__IDHoiDo__21B6055D");

                entity.HasOne(d => d.IdquanLyNavigation)
                    .WithMany(p => p.BoNhiem)
                    .HasForeignKey(d => d.IdquanLy)
                    .HasConstraintName("FK__BoNhiem__IDQuanL__20C1E124");
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdbaiPost).HasColumnName("IDBaiPost");

                entity.Property(e => e.IdnguoiTao).HasColumnName("IDNguoiTao");

                entity.Property(e => e.NoiDungComment).HasColumnType("date");

                entity.HasOne(d => d.IdbaiPostNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdbaiPost)
                    .HasConstraintName("FK__Comments__IDBaiP__403A8C7D");
            });

            modelBuilder.Entity<CtkenhThaoLuan>(entity =>
            {
                entity.ToTable("CTKenhThaoLuan");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IddeTai).HasColumnName("IDDeTai");

                entity.Property(e => e.IdkenhThaoLuan).HasColumnName("IDKenhThaoLuan");

                entity.HasOne(d => d.IddeTaiNavigation)
                    .WithMany(p => p.CtkenhThaoLuan)
                    .HasForeignKey(d => d.IddeTai)
                    .HasConstraintName("FK__CTKenhTha__IDDeT__3A81B327");

                entity.HasOne(d => d.IdkenhThaoLuanNavigation)
                    .WithMany(p => p.CtkenhThaoLuan)
                    .HasForeignKey(d => d.IdkenhThaoLuan)
                    .HasConstraintName("FK__CTKenhTha__IDKen__398D8EEE");
            });

            modelBuilder.Entity<CtxetDuyetVaDanhGia>(entity =>
            {
                entity.ToTable("CTXetDuyetVaDanhGia");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdnguoiTao).HasColumnName("IDNguoiTao");

                entity.Property(e => e.IdxetDuyet).HasColumnName("IDXetDuyet");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.HasOne(d => d.IdxetDuyetNavigation)
                    .WithMany(p => p.CtxetDuyetVaDanhGia)
                    .HasForeignKey(d => d.IdxetDuyet)
                    .HasConstraintName("FK__CTXetDuye__IDXet__30F848ED");
            });

            modelBuilder.Entity<DeTaiNghienCuu>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdgiangVien).HasColumnName("IDGiangVien");

                entity.Property(e => e.Idnhom).HasColumnName("IDNhom");

                entity.Property(e => e.NgayLap).HasColumnType("datetime");

                entity.HasOne(d => d.IdgiangVienNavigation)
                    .WithMany(p => p.DeTaiNghienCuu)
                    .HasForeignKey(d => d.IdgiangVien)
                    .HasConstraintName("FK__DeTaiNghi__IDGia__24927208");

                entity.HasOne(d => d.IdnhomNavigation)
                    .WithMany(p => p.DeTaiNghienCuu)
                    .HasForeignKey(d => d.Idnhom)
                    .HasConstraintName("FK__DeTaiNghi__IDNho__25869641");
            });

            modelBuilder.Entity<GiangVien>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ho).HasMaxLength(30);

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.Sdt)
                    .HasColumnName("SDT")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Ten).HasMaxLength(100);
            });

            modelBuilder.Entity<HoiDong>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NgayLap).HasColumnType("datetime");
            });

            modelBuilder.Entity<KenhThaoLuan>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdgiangVien).HasColumnName("IDGiangVien");

                entity.HasOne(d => d.IdgiangVienNavigation)
                    .WithMany(p => p.KenhThaoLuan)
                    .HasForeignKey(d => d.IdgiangVien)
                    .HasConstraintName("FK__KenhThaoL__IDGia__36B12243");
            });

            modelBuilder.Entity<MoDot>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdnamHoc).HasColumnName("IDNamHoc");

                entity.Property(e => e.IdquanLy).HasColumnName("IDQuanLy");

                entity.Property(e => e.ThoiGianBd)
                    .HasColumnName("ThoiGianBD")
                    .HasColumnType("datetime");

                entity.Property(e => e.ThoiGianKt)
                    .HasColumnName("ThoiGianKT")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.IdnamHocNavigation)
                    .WithMany(p => p.MoDot)
                    .HasForeignKey(d => d.IdnamHoc)
                    .HasConstraintName("FK__MoDot__IDNamHoc__286302EC");

                entity.HasOne(d => d.IdquanLyNavigation)
                    .WithMany(p => p.MoDot)
                    .HasForeignKey(d => d.IdquanLy)
                    .HasConstraintName("FK__MoDot__IDQuanLy__29572725");
            });

            modelBuilder.Entity<NamHoc>(entity =>
            {
                entity.HasIndex(e => new { e.HocKy, e.Nam })
                    .HasName("UNI_NamHoc")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HocKy)
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NhomSv>(entity =>
            {
                entity.ToTable("NhomSV");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdnamHoc).HasColumnName("IDNamHoc");

                entity.Property(e => e.TenNhom).HasMaxLength(50);

                entity.HasOne(d => d.IdnamHocNavigation)
                    .WithMany(p => p.NhomSv)
                    .HasForeignKey(d => d.IdnamHoc)
                    .HasConstraintName("FK__NhomSV__IDNamHoc__1367E606");
            });

            modelBuilder.Entity<QuanLy>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ho).HasMaxLength(30);

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.Sdt)
                    .HasColumnName("SDT")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Ten).HasMaxLength(100);
            });

            modelBuilder.Entity<SinhVien>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ho).HasMaxLength(30);

                entity.Property(e => e.Idnhom).HasColumnName("IDNhom");

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.Sdt)
                    .HasColumnName("SDT")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Ten).HasMaxLength(100);

                entity.HasOne(d => d.IdnhomNavigation)
                    .WithMany(p => p.SinhVien)
                    .HasForeignKey(d => d.Idnhom)
                    .HasConstraintName("FK__SinhVien__IDNhom__164452B1");
            });

            modelBuilder.Entity<XetDuyetVaDanhGia>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IddeTai).HasColumnName("IDDeTai");

                entity.Property(e => e.IdhoiDong).HasColumnName("IDHoiDong");

                entity.Property(e => e.IdmoDot).HasColumnName("IDMoDot");

                entity.HasOne(d => d.IddeTaiNavigation)
                    .WithMany(p => p.XetDuyetVaDanhGia)
                    .HasForeignKey(d => d.IddeTai)
                    .HasConstraintName("FK__XetDuyetV__IDDeT__2C3393D0");

                entity.HasOne(d => d.IdhoiDongNavigation)
                    .WithMany(p => p.XetDuyetVaDanhGia)
                    .HasForeignKey(d => d.IdhoiDong)
                    .HasConstraintName("FK__XetDuyetV__IDHoi__2D27B809");

                entity.HasOne(d => d.IdmoDotNavigation)
                    .WithMany(p => p.XetDuyetVaDanhGia)
                    .HasForeignKey(d => d.IdmoDot)
                    .HasConstraintName("FK__XetDuyetV__IDMoD__2E1BDC42");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
