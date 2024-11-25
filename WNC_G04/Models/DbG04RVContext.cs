using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace WNC_G04.Models
{
    public partial class DbG04RVContext : DbContext
    {
        internal DbSet<ChuDe> loaiChuDes;

        public DbG04RVContext()
        {
        }

        public DbG04RVContext(DbContextOptions<DbG04RVContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BaiViet> BaiViets { get; set; }
        public virtual DbSet<BinhLuan> BinhLuans { get; set; }
        public virtual DbSet<ChuDe> ChuDes { get; set; }
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; }
        public virtual DbSet<TheoDoi> TheoDois { get; set; }
        public virtual DbSet<Thich> Thiches { get; set; }
        public virtual DbSet<ThongBao> ThongBaos { get; set; }
        public object LoaiChuDes { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("G04RV")); // Cập nhật tên kết nối phù hợp
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaiViet>(entity =>
            {
                entity.HasKey(e => e.MaBaiViet);
                entity.ToTable("BaiViet");
                entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())").HasColumnType("datetime");

                entity.HasOne(d => d.MaChuDeNavigation)
                    .WithMany(p => p.BaiViets)
                    .HasForeignKey(d => d.MaChuDe)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.BaiViets)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<BinhLuan>(entity =>
            {
                entity.HasKey(e => e.MaBinhLuan);
                entity.ToTable("BinhLuan");
                entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())").HasColumnType("datetime");

                entity.HasOne(d => d.MaBaiVietNavigation)
                    .WithMany(p => p.BinhLuans)
                    .HasForeignKey(d => d.MaBaiViet)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.BinhLuans)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ChuDe>(entity =>
            {
                entity.HasKey(e => e.MaChuDe);
                entity.ToTable("ChuDe");
                entity.Property(e => e.TenChuDe).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasKey(e => e.MaNguoiDung);
                entity.ToTable("NguoiDung");
                entity.Property(e => e.TenNguoiDung).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MatKhau).IsRequired().HasMaxLength(256);
                entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.TieuSu).HasMaxLength(500);
            });

            modelBuilder.Entity<TheoDoi>(entity =>
            {
                entity.HasKey(e => e.MaTheoDoi);
                entity.ToTable("TheoDoi");
                entity.HasIndex(e => new { e.MaNguoiTheoDoi, e.MaNguoiDuocTheoDoi }).IsUnique();

                entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())").HasColumnType("datetime");

                entity.HasOne(d => d.MaNguoiDuocTheoDoiNavigation)
                    .WithMany(p => p.TheoDoiMaNguoiDuocTheoDoiNavigations)
                    .HasForeignKey(d => d.MaNguoiDuocTheoDoi)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MaNguoiTheoDoiNavigation)
                    .WithMany(p => p.TheoDoiMaNguoiTheoDoiNavigations)
                    .HasForeignKey(d => d.MaNguoiTheoDoi)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Thich>(entity =>
            {
                entity.HasKey(e => e.MaThich);
                entity.ToTable("Thich");

                entity.HasOne(d => d.MaBaiVietNavigation)
                    .WithMany(p => p.Thiches)
                    .HasForeignKey(d => d.MaBaiViet)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.Thiches)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ThongBao>(entity =>
            {
                entity.HasKey(e => e.MaThongBao);
                entity.ToTable("ThongBao");

                entity.Property(e => e.NoiDung).IsRequired().HasMaxLength(500);
                entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())").HasColumnType("datetime");

                entity.HasOne(d => d.MaBaiVietNavigation)
                   .WithMany(p => p.ThongBaos)
                   .HasForeignKey(d => d.MaBaiViet)
                   .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.ThongBaos)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    } }

