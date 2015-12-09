using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace SPS.Web.Ocr
{
    public class OcrEngine
    {
        private string _dataPath;

        public OcrEngine(string dataPath)
        {
            _dataPath = dataPath;
        }

        public string GetText(Bitmap image)
        {
            //var tesseract = new TesseractEngine(_dataPath, "eng", EngineMode.Default);
            //var pix = PixConverter.ToPix(new Bitmap(@"C:\Temp\imagem.jpeg"));
            var pix2 = Pix.LoadFromFile(@"C:\Temp\imagem.jpeg");

            pix2 = pix2.ConvertRGBToGray();
            pix2 = pix2.BinarizeOtsuAdaptiveThreshold(pix2.Width / 5, pix2.Height / 5, 10, 10, 0.1f);

            try
            {
                pix2.Save(@"C:\Temp\imagem2.jpeg");
            }
            catch
            {

            }

            //var page = tesseract.Process(pix);
            //var text = page.GetText();

            //page.Dispose();
            //tesseract.Dispose();
            var tesseract = new TesseractEngine(_dataPath, "eng", EngineMode.Default);

            var text2 = tesseract.Process(pix2).GetText();

            if (text2 == null)
            {

            }

            File.Delete(@"C:\Temp\imagem.jpeg");
            return text2;
        }
    }
}
