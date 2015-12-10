using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tesseract;

namespace SPS.Web.Ocr
{
    public class OcrEngine
    {
        private const string EngineLang = "eng";

        private string _dataPath;

        public OcrEngine(string dataPath)
        {
            _dataPath = dataPath;
        }

        public string GetText(Bitmap image)
        {
            using (var tesseract = new TesseractEngine(_dataPath, EngineLang, EngineMode.Default))
            {
                tesseract.SetVariable("tessedit_write_images", true);

                using (var pix = PixConverter.ToPix(image))
                {
                    using (var page = tesseract.Process(pix))
                    {
                        var text = RemoveSpecialCharacters(page.GetText());

#if DEBUG
                        System.Diagnostics.Debug.WriteLine("Confibialidade: " + page.GetMeanConfidence());
                        System.Diagnostics.Debug.WriteLine("Placa: " + text);
#endif
                        return text;
                    }
                }
            }
        }

        private string RemoveSpecialCharacters(string input)
        {
            var stringBuilder = new StringBuilder();

            foreach (var ch in input)
            {
                if (char.IsLetterOrDigit(ch) || ch == '-')
                {
                    stringBuilder.Append(ch);
                }
            }

            return new string(stringBuilder.ToString().Take(8).ToArray());
        }
    }
}
