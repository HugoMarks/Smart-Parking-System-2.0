using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace SPS.Raspberry.Extensions
{
    public static class ImageExtensions
    {
        public static async Task<string> ToBase64StringAsync(this WriteableBitmap writeableBitmap)
        {
            using (var stream = new InMemoryRandomAccessStream())
            {
                var bytes = new byte[stream.Size];
                      
                await writeableBitmap.ToStreamAsJpeg(stream);
                await stream.ReadAsync(bytes.AsBuffer(), (uint)stream.Size, InputStreamOptions.None);

                return Convert.ToBase64String(bytes, 0, bytes.Length);
            }
        }
    }
}
