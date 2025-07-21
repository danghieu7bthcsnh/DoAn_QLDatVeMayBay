using QLDatVeMayBay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace TestDatVeMayBay.Tests.Models
{
    public class NguoiDungTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void NguoiDung_HopLe_KhongCoLoi()
        {
            var nguoiDung = new NguoiDung
            {
                TenDangNhap = "huyen123",
                HoTen = "Đỗ Thị Huyền",
                Email = "huyen@example.com",
                SoDienThoai = "0912345678",
                GioiTinh = "Nữ",
                QuocTich = "Việt Nam",
                CCCD = "012345678912"
            };

            var results = ValidateModel(nguoiDung);
            Assert.Empty(results);
        }

        [Fact]
        public void NguoiDung_ThieuEmail()
        {
            var nguoiDung = new NguoiDung
            {
                TenDangNhap = "abc123"
                // thiếu Email
            };

            var results = ValidateModel(nguoiDung);
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void NguoiDung_EmailSaiDinhDang()
        {
            var nguoiDung = new NguoiDung
            {
                TenDangNhap = "abc123",
                Email = "khonghople.com"
            };

            var results = ValidateModel(nguoiDung);
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void NguoiDung_SoDienThoaiKhongHopLe()
        {
            var nguoiDung = new NguoiDung
            {
                TenDangNhap = "abc123",
                Email = "test@gmail.com",
                SoDienThoai = "abcdxyz" // không đúng định dạng số điện thoại
            };

            var results = ValidateModel(nguoiDung);
            Assert.Contains(results, r => r.MemberNames.Contains("SoDienThoai"));
        }

        [Fact]
        public void NguoiDung_TenDangNhapQuaDai()
        {
            var nguoiDung = new NguoiDung
            {
                TenDangNhap = new string('x', 51),
                Email = "abc@gmail.com"
            };

            var results = ValidateModel(nguoiDung);
            Assert.Contains(results, r => r.MemberNames.Contains("TenDangNhap"));
        }

        [Fact]
        public void NguoiDung_CCCDQuaDai()
        {
            var nguoiDung = new NguoiDung
            {
                TenDangNhap = "user",
                Email = "abc@gmail.com",
                CCCD = new string('1', 21) // vượt quá giới hạn 20 ký tự
            };

            var results = ValidateModel(nguoiDung);
            Assert.Contains(results, r => r.MemberNames.Contains("CCCD"));
        }
    }
}
