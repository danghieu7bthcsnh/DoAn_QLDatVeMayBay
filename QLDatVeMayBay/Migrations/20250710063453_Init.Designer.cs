﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QLDatVeMayBay.Data;

#nullable disable

namespace QLDatVeMayBay.Migrations
{
    [DbContext(typeof(QLDatVeMayBayContext))]
    [Migration("20250710063453_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QLDatVeMayBay.Models.ChuyenBay", b =>
                {
                    b.Property<int>("IDChuyenBay")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDChuyenBay"));

                    b.Property<decimal>("GiaVe")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("GioCatCanh")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("GioHaCanh")
                        .HasColumnType("datetime2");

                    b.Property<int>("IDMayBay")
                        .HasColumnType("int");

                    b.Property<int>("SanBayDen")
                        .HasColumnType("int");

                    b.Property<int>("SanBayDi")
                        .HasColumnType("int");

                    b.Property<string>("TinhTrang")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IDChuyenBay");

                    b.HasIndex("IDMayBay");

                    b.HasIndex("SanBayDen");

                    b.HasIndex("SanBayDi");

                    b.ToTable("ChuyenBay");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.GheNgoi", b =>
                {
                    b.Property<int>("IDGhe")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDGhe"));

                    b.Property<string>("HangGhe")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("IDChuyenBay")
                        .HasColumnType("int");

                    b.Property<string>("TrangThai")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("IDGhe");

                    b.HasIndex("IDChuyenBay");

                    b.ToTable("GheNgoi");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.LoaiMayBay", b =>
                {
                    b.Property<string>("LoaiMayBayId")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MoTa")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("TongSoGhe")
                        .HasColumnType("int");

                    b.HasKey("LoaiMayBayId");

                    b.ToTable("LoaiMayBay");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.MaXacNhan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Ma")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenDangNhap")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ThoiGianHetHan")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("MaXacNhan");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.MayBay", b =>
                {
                    b.Property<int>("IDMayBay")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDMayBay"));

                    b.Property<string>("LoaiMayBayId")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TenHangHK")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IDMayBay");

                    b.HasIndex("LoaiMayBayId");

                    b.ToTable("MayBay");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.NguoiDung", b =>
                {
                    b.Property<int>("IDNguoiDung")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDNguoiDung"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("GioiTinh")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SoDienThoai")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TenDangNhap")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IDNguoiDung");

                    b.HasIndex("TenDangNhap")
                        .IsUnique();

                    b.ToTable("NguoiDung");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.SanBay", b =>
                {
                    b.Property<int>("IDSanBay")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDSanBay"));

                    b.Property<string>("DiaDiem")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TenSanBay")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IDSanBay");

                    b.ToTable("SanBay");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.TaiKhoan", b =>
                {
                    b.Property<string>("TenDangNhap")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TrangThaiTK")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("VaiTro")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("TenDangNhap");

                    b.ToTable("TaiKhoan");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.ThanhToan", b =>
                {
                    b.Property<int>("IDThanhToan")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDThanhToan"));

                    b.Property<int>("IDVe")
                        .HasColumnType("int");

                    b.Property<string>("PhuongThuc")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("SoTien")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ThoiGianGiaoDich")
                        .HasColumnType("datetime2");

                    b.Property<string>("TrangThaiThanhToan")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IDThanhToan");

                    b.HasIndex("IDVe");

                    b.ToTable("ThanhToan");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.VeMayBay", b =>
                {
                    b.Property<int>("IDVe")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDVe"));

                    b.Property<int>("IDChuyenBay")
                        .HasColumnType("int");

                    b.Property<int>("IDGhe")
                        .HasColumnType("int");

                    b.Property<int>("IDNguoiDung")
                        .HasColumnType("int");

                    b.Property<DateTime>("ThoiGianDat")
                        .HasColumnType("datetime2");

                    b.Property<string>("TrangThaiVe")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IDVe");

                    b.HasIndex("IDChuyenBay");

                    b.HasIndex("IDNguoiDung");

                    b.ToTable("VeMayBay");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.ChuyenBay", b =>
                {
                    b.HasOne("QLDatVeMayBay.Models.MayBay", "MayBay")
                        .WithMany()
                        .HasForeignKey("IDMayBay")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("QLDatVeMayBay.Models.SanBay", "SanBayDenInfo")
                        .WithMany()
                        .HasForeignKey("SanBayDen")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("QLDatVeMayBay.Models.SanBay", "SanBayDiInfo")
                        .WithMany()
                        .HasForeignKey("SanBayDi")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("MayBay");

                    b.Navigation("SanBayDenInfo");

                    b.Navigation("SanBayDiInfo");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.GheNgoi", b =>
                {
                    b.HasOne("QLDatVeMayBay.Models.ChuyenBay", "ChuyenBay")
                        .WithMany()
                        .HasForeignKey("IDChuyenBay")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChuyenBay");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.MayBay", b =>
                {
                    b.HasOne("QLDatVeMayBay.Models.LoaiMayBay", "LoaiMayBay")
                        .WithMany()
                        .HasForeignKey("LoaiMayBayId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("LoaiMayBay");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.NguoiDung", b =>
                {
                    b.HasOne("QLDatVeMayBay.Models.TaiKhoan", "TaiKhoan")
                        .WithOne("NguoiDung")
                        .HasForeignKey("QLDatVeMayBay.Models.NguoiDung", "TenDangNhap")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TaiKhoan");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.ThanhToan", b =>
                {
                    b.HasOne("QLDatVeMayBay.Models.VeMayBay", "VeMayBay")
                        .WithMany()
                        .HasForeignKey("IDVe")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VeMayBay");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.VeMayBay", b =>
                {
                    b.HasOne("QLDatVeMayBay.Models.ChuyenBay", "ChuyenBay")
                        .WithMany()
                        .HasForeignKey("IDChuyenBay")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("QLDatVeMayBay.Models.NguoiDung", "NguoiDung")
                        .WithMany()
                        .HasForeignKey("IDNguoiDung")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChuyenBay");

                    b.Navigation("NguoiDung");
                });

            modelBuilder.Entity("QLDatVeMayBay.Models.TaiKhoan", b =>
                {
                    b.Navigation("NguoiDung");
                });
#pragma warning restore 612, 618
        }
    }
}
