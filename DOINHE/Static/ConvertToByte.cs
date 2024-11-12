using System.IO;

namespace DOINHE.Static
{
    public static class ConvertToByte
    {
        public static byte[] ConvertIFormFileToByte(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                
                return memoryStream.ToArray();
            }
        }

        public static IFormFile ConvertByteToIFormFile(byte[] img, string name)
        {
            using (var stream = new MemoryStream(img))
            {
                IFormFile file = new FormFile(stream, 0, img.Length, name, "img")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };
                return file;
            }
               
        }

        internal static byte[] ConvertIFormFileToByte(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
