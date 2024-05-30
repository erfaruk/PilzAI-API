using SixLabors.ImageSharp;

namespace PilzGPTArge_V0.Helper
{
    public class ImageHelper
    {
        public static string? ByteToString(byte[]? image)
        {
            try
            {
                if (image == null) return null;
                string imageFormat = GetImageFormat(image);
                return $"data:image/{imageFormat};base64," + Convert.ToBase64String(image); // fotoğrafı base64 kodu yapar
            }
            catch
            {
                return null;
            }


        }


        public static byte[]? StringToByte(string? image)
        {
            try
            {
                if (image == null) return null;

                var splitedString = image.Split(','); //görselin base64 string kodunu virgülden ikiye bölür. [0] --> data:image; base64   ve  [1] --> görselin kodları
                return Convert.FromBase64String(splitedString[1]);

            }
            catch
            { return null; }
        }

        public static byte[]? Compress(byte[]? data)
        {
            if (data == null) return null;
            try
            {
                using var inputStream = new MemoryStream(data);  //Ram de olan fotoğrafın datasını byte olarak inputStream de tut.
                //SixLabors.ImageSharp eklentiyi nuget manager de bulup kurunuz.
                using var image = Image.Load(inputStream);  //sixLabors eklentisi byte olan fotoğrafınızı çevirip image içinde tutuyor.

                SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder encoder = new() //fotoyu 40% kalitesini sıkıştırıp düşüyor ve aynı zamanda jpeg olarak kayıt ediyor.
                {
                    Quality = 40
                };
                using var outputStream = new MemoryStream();
                image.Save(outputStream, encoder);
                var outputBytes = outputStream.ToArray();
                return outputBytes;
            }
            catch { return null; }
        }

        public static string GetImageFormat(byte[] image)
        {
            if (image.Length < 12)
            {
                return "Unknown";
            }

            using (MemoryStream ms = new MemoryStream(image))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    // Read the first 12 bytes of the image
                    byte[] headerBytes = br.ReadBytes(12);

                    // Check the image format
                    if (headerBytes[0] == 0xFF && headerBytes[1] == 0xD8 && headerBytes[2] == 0xFF)
                    {
                        return "jpeg";
                    }
                    else if (headerBytes[0] == 0x89 && headerBytes[1] == 0x50 && headerBytes[2] == 0x4E && headerBytes[3] == 0x47)
                    {
                        return "png";
                    }
                    else if (headerBytes[0] == 0x47 && headerBytes[1] == 0x49 && headerBytes[2] == 0x46)
                    {
                        return "gif";
                    }
                    else if (headerBytes[0] == 0x42 && headerBytes[1] == 0x4D)
                    {
                        return "bmp";
                    }
                    else if (headerBytes[0] == 0x49 && headerBytes[1] == 0x49 && headerBytes[2] == 0x2A && headerBytes[3] == 0x00)
                    {
                        return "tiff";
                    }
                    else if (headerBytes[0] == 0x4D && headerBytes[1] == 0x4D && headerBytes[2] == 0x00 && headerBytes[3] == 0x2A)
                    {
                        return "tiff";
                    }
                    else
                    {
                        return "Unknown";
                    }
                }
            }
        }
    }
}
