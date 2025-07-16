using QRCoder;

namespace QLDatVeMayBay.Helper
{
    public class QRCodeHelper
    {
        public static string GenerateQRCodeBase64(string content)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            var qrBytes = qrCode.GetGraphic(20);
            return Convert.ToBase64String(qrBytes);
        }
    }
}