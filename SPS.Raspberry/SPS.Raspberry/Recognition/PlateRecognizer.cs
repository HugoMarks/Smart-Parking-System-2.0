using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using AForgePoint = AForge.Point;

namespace SPS.Raspberry.Recognition
{
    public class PlateRecognizer
    {
        private const double MaxPlateRatio = 0.4;

        private const double MinPlateRatio = 0.2;

        public async Task<WriteableBitmap> RecognizePlateAsync(IRandomAccessStream imageStream)
        {
            var writeableBitmap = await WriteableBitmapExtensions.FromStream(null, imageStream);

            await SaveImageAsync(writeableBitmap);

            var processedImage = GetEdgedImage(writeableBitmap.Clone());            
            var possiblePlatePositions = await GetPlateRectanglesAsync(processedImage);
            var plate = FindPlate(possiblePlatePositions, writeableBitmap);

            if (plate == null)
            {
                return null;
            }

            await SaveImageAsync(plate);

            return plate;
        }

        private async Task SaveImageAsync(WriteableBitmap bitmap)
        {
            var folder = KnownFolders.PicturesLibrary;
            var file = await folder.CreateFileAsync("placa.jpeg", CreationCollisionOption.GenerateUniqueName);

            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                await bitmap.ToStreamAsJpeg(stream);
            }
        }

        private WriteableBitmap FindPlate(IEnumerable<Rect> rects, WriteableBitmap image)
        {
            WriteableBitmap bestCandidate = null;
            
            foreach (var rect in rects)
            {
                var croppedImage = image.Crop(rect);
                var edgeFilter = new CannyEdgeDetector();
                var smoothFilter = new Median();
                var grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);
                var blobCounter = new BlobCounter();
                var cutTop = croppedImage.PixelHeight * 0.3;

                croppedImage = croppedImage.Crop(new Rect(0, cutTop, croppedImage.PixelWidth, croppedImage.PixelHeight));

                var bitmap = (Bitmap)croppedImage;
                var grayImage = grayFilter.Apply(bitmap);

                bitmap = smoothFilter.Apply(grayImage);
                edgeFilter.ApplyInPlace(bitmap);
                blobCounter.ProcessImage(bitmap);

                var blobs = blobCounter.GetObjectsInformation();
                var possibleChars = new List<Rectangle>();

                foreach (var blob in blobs)
                {
                    var objRectangle = blob.Rectangle;
                    var ratio = (double)objRectangle.Height / (double)objRectangle.Width;
                    
                    if (ratio >= 1.16d && ratio <= 6.3d)
                    {
                        possibleChars.Add(objRectangle);
                    }
                }

                if (possibleChars.Count == 0)
                {
                    continue;
                }

                bestCandidate = croppedImage;
            }

            return bestCandidate;
        }

        private Bitmap GetEdgedImage(WriteableBitmap writeableBitmap)
        {
            var edgeFilter = new CannyEdgeDetector(255, 0);
            var smoothFilter = new Median();
            var grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            var bitmap = (Bitmap)writeableBitmap;

            bitmap = grayFilter.Apply(bitmap);
            smoothFilter.ApplyInPlace(bitmap);
            edgeFilter.ApplyInPlace(bitmap);

            return bitmap;
        }

        private async Task<IList<Rect>> GetPlateRectanglesAsync(Bitmap image)
        {
            var rectanglePoints = await Task.Factory.StartNew(() =>
            {
                var blobCounter = new BlobCounter
                {
                    FilterBlobs = true,
                    MinHeight = 5,
                    MinWidth = 5
                };

                blobCounter.ProcessImage(image);

                var blobs = blobCounter.GetObjectsInformation();
                var shapeChecker = new SimpleShapeChecker();
                var rectPoints = new List<List<AForgePoint>>();

                foreach (var blob in blobs)
                {
                    List<IntPoint> cornerPoints;
                    var edgePoints = blobCounter.GetBlobsEdgePoints(blob);
                    var points = new List<AForgePoint>();

                    if (shapeChecker.IsQuadrilateral(edgePoints, out cornerPoints))
                    {
                        var polygonType = shapeChecker.CheckPolygonSubType(cornerPoints);

                        if (polygonType == PolygonSubType.Rectangle ||
                            polygonType == PolygonSubType.Parallelogram)
                        {
                            foreach (var point in cornerPoints)
                            {
                                points.Add(new AForgePoint(point.X, point.Y));
                            }

                            rectPoints.Add(points);
                        }
                    }
                }

                return rectPoints;
            });

            var rects = rectanglePoints.Select(points => GetRect(points)).ToList();
            var images = new List<WriteableBitmap>();
            var cadidatesRects = new List<Rect>();

            foreach (var rect in rects)
            {
                var ratio = rect.Height / rect.Width;

                if (ratio >= MinPlateRatio && ratio <= MaxPlateRatio)
                {
                    cadidatesRects.Add(rect);
                }
            }

            return cadidatesRects;
        }

        private Rect GetRect(IEnumerable<AForgePoint> points)
        {
            var xMin = (int)points.Min(s => s.X);
            var yMin = (int)points.Min(s => s.Y);
            var xMax = (int)points.Max(s => s.X);
            var yMax = (int)points.Max(s => s.Y);
            var rect = new Rect(xMin, yMin, xMax - xMin, yMax - yMin);

            return rect;
        }
    }
}
