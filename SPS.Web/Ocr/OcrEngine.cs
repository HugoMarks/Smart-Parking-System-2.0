using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace SPS.Web.Ocr
{
    public class OcrEngine
    {
        public OcrEngine()
        {

        }

        public string GetText(Bitmap image)
        {
            var pixImage = PixConverter.ToPix(image);
            var tesseract = new Tesseract();

            tesseract.
        }
    }
}
