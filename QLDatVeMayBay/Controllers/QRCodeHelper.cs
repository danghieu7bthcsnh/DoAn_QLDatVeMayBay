using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace QLDatVeMayBay.Helpers
{
    public class QRCodeHelper
    {
        //public static string GenerateQRCodeBase64(string content)
        //{
        //    using var qrGenerator = new QRCodeGenerator();
        //    using var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        //    using var qrCode = new QRCode(qrCodeData);
        //    using var bitmap = qrCode.GetGraphic(20);
        //    using var ms = new MemoryStream();
        //    bitmap.Save(ms, ImageFormat.Png);
        //    return $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";
        //}
    }
}
