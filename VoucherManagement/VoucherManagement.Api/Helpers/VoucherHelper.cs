using iText.Barcodes;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using QRCoder;
using System.Security.Cryptography;
using System.Text;
using VoucherManagement.Api.Models;

namespace VoucherManagement.Api.Helpers
{
    public class VoucherHelper
    {
        public static string GenerateSecureId(int length = 10)
        {
            string AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

            var result = new StringBuilder(length);

            // Nutzt den kryptografisch sicheren Zufall von .NET
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] uintBuffer = new byte[4];

                while (result.Length < length)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);

                    // Modulo-Operation zur Auswahl des Zeichens
                    result.Append(AllowedChars[(int)(num % (uint)AllowedChars.Length)]);
                }
            }

            return result.ToString();
        }

        public static byte[] GeneratePdfWithIText(Voucher voucher, string templatePath, int pdfPage=1, float qrCodeSize = 60,int fontSize = 5, int spacingRight = 45, int spacingBottom = 40, int textSpacingBottom = 45)
        {
            using var ms = new MemoryStream();
            using var reader = new PdfReader(new MemoryStream(System.IO.File.ReadAllBytes(templatePath)));
            using var writer = new PdfWriter(ms);
            using var pdfDoc = new PdfDocument(reader, writer);

            // QR-Code mit iText BarcodeQRCode erstellen (benötigt BouncyCastle)
            var qrCode = new BarcodeQRCode(voucher.Code);

            // Seite 2 sicherstellen
            PdfPage page2;
            page2 = pdfDoc.GetPage(pdfPage);

            // QR-Code direkt auf Canvas zeichnen
            var canvas = new PdfCanvas(page2);
            float qrSize = qrCodeSize;
            float margin = spacingRight; // Mehr Abstand vom rechten Rand

            // Position: unten rechts
            float x = page2.GetPageSize().GetWidth() - qrSize - margin;
            float y = spacingBottom; // unten

            // Matrix für Positionierung setzen BEVOR der QR-Code gezeichnet wird
            canvas.SaveState();
            canvas.ConcatMatrix(1, 0, 0, 1, x, y);

            // QR-Code platzieren
            qrCode.PlaceBarcode(canvas,
                iText.Kernel.Colors.ColorConstants.BLACK,
                qrSize / qrCode.GetBarcodeSize().GetWidth());

            canvas.RestoreState();

            // Text unter dem QR-Code hinzufügen
            var document = new Document(pdfDoc);
            float textY = textSpacingBottom; // Näher am QR-Code (war 0, jetzt 8 Punkte darunter)
            float textX = x + (qrSize / 2); // zentriert unter dem QR-Code

            var paragraph = new Paragraph(voucher.Code)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(fontSize)
                .SetTextAlignment(TextAlignment.CENTER);

            document.ShowTextAligned(paragraph, textX, textY, pdfPage,
                TextAlignment.CENTER, VerticalAlignment.TOP, 0);

            document.Close();

            return ms.ToArray();
        }
    }
}

