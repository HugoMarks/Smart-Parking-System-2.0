using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace SPS.Raspberry.Core.Camera
{
    public class PhotoCam
    {
        private MediaCapture _mediaCapture;

        public PhotoCam()
        {
            _mediaCapture = new MediaCapture();
        }

        public async Task InitAsync(CaptureElement captureElement)
        {
            await _mediaCapture.InitializeAsync();            
            captureElement.Source = _mediaCapture;
            await _mediaCapture.StartPreviewAsync();
        }

        public async Task TakePhotoToStreamAsync(IRandomAccessStream srcStream)
        {
            await _mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), srcStream);
        }
        private static async Task<DeviceInformation> GetCameraID(Windows.Devices.Enumeration.Panel desired)
        {
            DeviceInformation deviceID = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture))
                .FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == desired);

            if (deviceID != null) return deviceID;
            else throw new Exception(string.Format("Camera of type {0} doesn't exist.", desired));
        }
    }
}
