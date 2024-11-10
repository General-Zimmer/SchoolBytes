using System;
using System.Drawing;
using System.IO;
using QRCoder;

/// <summary>
/// Class <c>QRGeneratorService</c> is a service class that implements the package QRCoder
/// to generate QR Codes for redirection to specific webpages
/// </summary>
namespace QRService
{
    public class QRGeneratorService
    {
        private readonly QRCodeGenerator qrGenerator = new QRCodeGenerator(); // field responsible for generating QR codes

        /// <summary>
        /// This method will generate a Bitmap from the given string, typically a URL. This Bitmap
        /// can be used to generate an Image with a readable QRCode.
        /// </summary>
        /// <returns>A Bitmap object</returns>
        public Bitmap GenerateBitmap(string content)
        {
            using (qrGenerator)
            {
                // qrCodeData is the information, generated from the content parameter, to be rendered. The ECCLevel
                // sets how much of the data may be corrupted before it fails to load. Q sets it to 25%
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);
                using (qrCode)
                {
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    return qrCodeImage;
                }
            }
        }

        /// <summary>
        /// Method that saves a QRCode as a png to the given filepath
        /// </summary>
        public void SaveQRCodeAsPng(string content, string filePath)
        {
            Bitmap qrCodeImage = GenerateBitmap(content);

            try
            {
                // Save the Bitmap as a PNG file
                qrCodeImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                Console.WriteLine("Image saved");
                qrCodeImage.Dispose();
            }
            catch (IOException ex)
            {
                // Handle any I/O errors here
                Console.WriteLine($"An error occurred while saving the QR code: {ex.Message}");
            }
        }

    }
}